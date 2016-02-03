using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("Scripts/Weapon/Projectile Behaviour")]
public class ProjectileBehaviour : PoolableBehaviour {

    // settings
    public float speed;
    public float lifetime;
    public float damage;
    public float force;

    public bool useRadius = false;
    public float radius = 0f;

    public ParticleBehaviour hitParticles;

    // private state
    private Vector3 direction;
    private DamageDealer dealer;
    private float lifeRemaining;
    private Vector3 raycastGoal;
    private bool alive;

    private ProjectileEffect[] cachedEffects;
    private ProjectileEffect[] effects {
        get {
            if (cachedEffects == null) {
                cachedEffects = GetComponentsInChildren<ProjectileEffect>(true);
            }
            return cachedEffects;
        }
    }

    // readonly properties
    public Vector3 PhysicsPosition {
        get { return raycastGoal; }
    }

    public bool IsAlive {
        get { return alive; }
    }

    // Call to add projectile to scene, this actually starts the projectile moving
    public override void Activate() {
        lifeRemaining = lifetime;
        raycastGoal = transform.position;

        gameObject.SetActive(true);

        // activate effects
        Array.ForEach(effects, e => e.OnActivate(direction));
    }

    // Call to remove projectile from scene.
    public override void Deactivate() {
        gameObject.SetActive(false);

        // deactivate effects
        Array.ForEach(effects, e => e.OnDeactivate());
    }

    // Initialize bullet with direction and dealer, then activate
    public void InitializeAndActivate(Vector3 initialPosition, Vector3 direction, DamageDealer dealer) {
        // validate radius
        if (useRadius && radius <= 0f) {
            Debug.LogError("Can't use radius of 0 or less: " + radius + ", ignoring", this);
            useRadius = false;
        }

        // move forward by radius if applicable, prevents unexpected collisions
        if (useRadius) {
            transform.position = initialPosition + direction * radius;
        } else {
            transform.position = initialPosition;
        }
        
        this.direction = direction;
        this.dealer = dealer;
        alive = true;

        Activate();
    }

    // Called after all Updates; Match the graphical position with the physics position
    void LateUpdate() {
        UpdateLifeTimer(Time.deltaTime);
        MoveToRaycastGoal(Time.deltaTime);
    }

    // Called after physics update; here we check for collisions
    void FixedUpdate() {
        PhysicsUpdate(Time.deltaTime);
    }

    // handle bullet lifetime
    private void UpdateLifeTimer(float time) {
        if (alive) {
            lifeRemaining -= time;
            if (lifeRemaining <= 0f) {
                StartCoroutine(StopProjectile());
            }
        }
    }

    // move game object towards the raycasted goal position
    private void MoveToRaycastGoal(float time) {
        if (alive) {
            float maxDist = speed * time;
            Vector3 toGoal = raycastGoal - transform.position;
            if (toGoal.sqrMagnitude <= maxDist * maxDist) {
                transform.position = raycastGoal;
            } else {
                transform.position += toGoal.normalized * maxDist;
            }
        } else {
            // in case we hit something, just move to the goal position and await deactivation
            transform.position = raycastGoal;
        }
    }

    // move projectile and check for collisions
    private void PhysicsUpdate(float time) {
        if (alive) {
            RaycastHit hit;
            if (CheckNextSegment(out hit)) {
                raycastGoal += direction * hit.distance;
                Hit(hit);
            } else {
                raycastGoal += direction * speed * time;
            }
        }
    }

    // Check a segment of bullet movement.  A segment is the distance the bullet moves in one physics update
    private bool CheckNextSegment(out RaycastHit hit) {
        if (useRadius) {
            return Physics.SphereCast(raycastGoal, radius, direction, out hit, speed * Time.deltaTime);
        } else {
            return Physics.Raycast(raycastGoal, direction, out hit, speed * Time.deltaTime);
        }
    }

    // Handle the bullet hitting something
    private void Hit(RaycastHit hit) {
        Debug.Log("Projectile hit: " + hit.collider, this);

        // apply damage
        if (hit.transform.ApplyDamage(damage, hit.point, direction * force, dealer)) {
            Debug.Log("Applied damage to: " + hit.transform, this);
        }

        // spawn hit particles
        if (hitParticles != null) {
            ParticleBehaviour p = (ParticleBehaviour)PoolManager.Instance.GetPooledObject(hitParticles);
            p.transform.position = hit.point;
            p.transform.LookAt(hit.point + hit.normal);
            p.Activate();
        }

        StartCoroutine(StopProjectile());
    }

    // handle cleaning up the projectile and disabling any effects
    private IEnumerator StopProjectile() {
        if (!alive) {
            yield break;
        }

        lifeRemaining = 0f;
        alive = false;

        // find max effect delay
        float delay = 0f;
        Array.ForEach(effects, e => {
            delay = Mathf.Max(delay, e.CleanUpTime);
        });

        // tell effects to cleanup
        Array.ForEach(effects, e => e.CleanUp());

        // do nothing while effects cleanup
        yield return new WaitForSeconds(delay);

        // check for any effects not yet cleaned up
        while (Array.Exists(effects, e => !e.IsCleanedUp)) {
            yield return null;
        }

        ReturnToPool();
    }
}

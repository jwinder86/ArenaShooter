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

    // private state
    private Vector3 direction;
    private DamageDealer dealer;
    private float lifeRemaining;

    private Vector3 raycastGoal;
    public Vector3 PhysicsPosition {
        get { return raycastGoal; }
    }

    private bool alive;
    public bool IsAlive {
        get { return alive; }
    }

    // Call to add projectile to scene, this actually starts the projectile moving
    public override void Activate() {
        lifeRemaining = lifetime;
        raycastGoal = transform.position;

        gameObject.SetActive(true);
    }

    // Call to remove projectile from scene.
    public override void Deactivate() {
        gameObject.SetActive(false);
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
                StopProjectile();
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

        StopProjectile();
    }

    // handle cleaning up the projectile and disabling any effects
    private void StopProjectile() {
        lifeRemaining = 0f;
        alive = false;

        //TODO: wait for any effects to complete here!

        ReturnToPool();
    }
}

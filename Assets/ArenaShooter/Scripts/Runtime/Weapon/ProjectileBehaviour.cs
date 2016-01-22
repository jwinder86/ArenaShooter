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
    private bool alive;

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
            Debug.LogError("Can't use radius of 0 or less: " + radius + ", ignoring");
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
        if (alive) {
            lifeRemaining -= Time.deltaTime;

            // if time over, destroy
            if (lifeRemaining < 0f) {
                ReturnToPool();
                return;
            }
        }

        // try to move to goal, assumes that raycastGoal is in direction specified
        float maxDist = speed * Time.deltaTime;
        Vector3 toGoal = raycastGoal - transform.position;
        if (toGoal.sqrMagnitude <= maxDist * maxDist) {
            transform.position = raycastGoal;
        } else {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    // Called after physics update; here we check for collisions
    void FixedUpdate() {
        if (alive) {
            RaycastHit hit;
            if (checkNextSegment(out hit)) {
                //Debug.Log("Bullet hit " + hit.transform.gameObject + " at " + hit.point);
                raycastGoal += direction * hit.distance;
                Hit(hit);
            } else {
                raycastGoal += direction * speed * Time.deltaTime;
            }
        }
    }

    // Check a segment of bullet movement.  A segment is the distance the bullet moves in one physics update
    private bool checkNextSegment(out RaycastHit hit) {
        if (useRadius) {
            return Physics.SphereCast(raycastGoal, radius, direction, out hit, speed * Time.deltaTime);
        } else {
            return Physics.Raycast(raycastGoal, direction, out hit, speed * Time.deltaTime);
        }
    }

    // Handle the bullet hitting something
    private void Hit(RaycastHit hit) {
        // apply damage
        if (hit.transform.ApplyDamage(damage, hit.point, direction * force, dealer)) {
            Debug.Log("Applied damage to: " + hit.transform);
        }

        ReturnToPool();
    }
}

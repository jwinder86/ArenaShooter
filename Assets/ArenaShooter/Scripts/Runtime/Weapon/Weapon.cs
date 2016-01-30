using UnityEngine;
using System.Collections;

public class Weapon {

    private WeaponConfig config;
    private float fireIntervalRemaining;
    public bool CanFire {
        get { return fireIntervalRemaining <= 0f; }
    }

    public Weapon(WeaponConfig config) {
        this.config = config;
        fireIntervalRemaining = 0f;
    }

    // Called to update weapon's internal delay timers
    public void UpdateTimer(float time) {
        if (fireIntervalRemaining > 0f) {
            fireIntervalRemaining -= time;
            fireIntervalRemaining = Mathf.Max(fireIntervalRemaining, 0f);
        }
    }

    // Fire weapon, if not in the delay interval
    public bool FireWeapon(Vector3 position, Vector3 direction, DamageDealer dealer) {
        if (fireIntervalRemaining <= 0f) {
            Vector3 fireDirection = randomOffset(direction, config.sprayAngle);
            ProjectileBehaviour projectile = (ProjectileBehaviour)PoolManager.Instance.GetPooledObject(config.projectilePrefab);
            projectile.InitializeAndActivate(position, fireDirection, dealer);

            // set the interval to prevent firing immediately on the next frame
            fireIntervalRemaining = config.fireInterval;

            return true;
        } else {
            return false;
        }
    }

    // Rotate input vector by a random amount up to sprayDegrees
    private Vector3 randomOffset(Vector3 input, float sprayAngle) {
        return Quaternion.Lerp(Quaternion.identity, Random.rotation, sprayAngle / 360f) * input;
    }
}

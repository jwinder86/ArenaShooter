using UnityEngine;
using System.Collections;

// Class to store weapon configuration as a Unity asset
public class WeaponConfig : ScriptableObject {
    public ProjectileBehaviour projectilePrefab;
    public float fireInterval;
    public float sprayAngle;

    public bool Validate() {
        if (projectilePrefab == null) {
            Debug.LogError("No projectile set for weapon", this);
            return false;
        }

        if (fireInterval <= 0f) {
            Debug.LogError("Fire Interval (" + fireInterval + ") must be a positive number", this);
            return false;
        }

        if (sprayAngle < 0f) {
            Debug.LogError("Spray Angle (" + fireInterval + ") cannot be negative", this);
            return false;
        }

        return true;
    }
}

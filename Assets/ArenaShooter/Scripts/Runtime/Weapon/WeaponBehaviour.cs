using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Weapon/Weapon Behaviour")]
public class WeaponBehaviour : MonoBehaviour {

    public ProjectileBehaviour projectilePrefab;

    public float fireInterval;
    public float sprayAngle;

    private bool inFireInterval;

    public bool CanFire {
        get { return !inFireInterval; }
    }

	// Use this for initialization
	void Start () {
	    if (fireInterval <= 0f) {
            Debug.LogError("Interval between bullets (" + fireInterval + ") must be more than 0s");
            fireInterval = 0.5f; // default to 2 per second
        }

        inFireInterval = false;
	}
	
    // Fire weapon, if not in the delay interval
	public bool FireWeapon(Vector3 position, Vector3 direction, DamageDealer dealer) {
        if (!inFireInterval) {
            Vector3 fireDirection = randomOffset(direction, sprayAngle);
            ProjectileBehaviour projectile = (ProjectileBehaviour) PoolManager.Instance.GetPooledObject(projectilePrefab);
            projectile.InitializeAndActivate(position, fireDirection, dealer);

            StartCoroutine(FireDelay());

            return true;
        } else {
            return false;
        }
    }

    // Lock firing weapon for duration of interval
    private IEnumerator FireDelay() {
        inFireInterval = true;
        yield return new WaitForSeconds(fireInterval);
        inFireInterval = false;
    }

    // Rotate input vector by a random amount up to sprayDegrees
    private Vector3 randomOffset(Vector3 input, float sprayAngle) {
        return Quaternion.Lerp(Quaternion.identity, Random.rotation, sprayAngle / 360f) * input;
    }
}

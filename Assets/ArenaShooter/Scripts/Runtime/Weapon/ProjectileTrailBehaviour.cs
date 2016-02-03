using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Weapon/Projectile Trail Behaviour")]
[RequireComponent (typeof(TrailRenderer))]
public class ProjectileTrailBehaviour : MonoBehaviour, ProjectileEffect {

    private TrailRenderer trail;
    private bool isCleanedUp;

    public bool IsCleanedUp {
        get { return isCleanedUp; }
    }

    public float CleanUpTime {
        get { return trail.time; }
    }

	void Awake() {
        trail = GetComponent<TrailRenderer>();
    }

    public void OnActivate(Vector3 direction) {
        trail.enabled = true;
        isCleanedUp = false;
    }

    public void OnDeactivate() {
        trail.enabled = false;
    }

    public void CleanUp() {
        StopAllCoroutines();
        StartCoroutine(CleanUpRoutine());
    }

    private IEnumerator CleanUpRoutine() {
        // wait for trail to reach end
        yield return new WaitForSeconds(trail.time);

        // force trail cleanup by letting frame pass with negative duration
        float defaultTrailTime = trail.time;
        trail.time = -1f;
        yield return null;
        trail.time = defaultTrailTime;

        OnDeactivate();

        isCleanedUp = true;
    }
}

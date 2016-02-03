using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("Scripts/Weapon/Projectile Model Behaviour")]
[RequireComponent(typeof(MeshRenderer))]
public class ProjectileModelBehaviour : MonoBehaviour, ProjectileEffect {

    new private MeshRenderer renderer;

    void Awake() {
        renderer = GetComponent<MeshRenderer>();
    }

    public float CleanUpTime {
        get { return 0f; }
    }

    public bool IsCleanedUp {
        get {
            return !renderer.enabled;
        }
    }

    public void CleanUp() {
        renderer.enabled = false;
    }

    public void OnActivate(Vector3 direction) {
        renderer.enabled = true;

        transform.LookAt(transform.position + direction);
    }

    public void OnDeactivate() {
        renderer.enabled = false;
    }
}

using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(DamageReceiver))]
[RequireComponent (typeof(HealthBehaviour))]
[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(CapsuleCollider))]
[AddComponentMenu ("Scripts/Player/Player Manager")]
public class PlayerManager : MonoBehaviour, DeathHandler, DamageHandler {

    private CharacterController controller;
    new private Rigidbody rigidbody;
    new private CapsuleCollider collider;

    private bool alive;
    public bool Alive {
        get { return alive; }
    }

    void Awake() {
        controller = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
    }

    void Start () {
        alive = true;

        // default to using character controller
        controller.enabled = true;
        rigidbody.isKinematic = true;
        collider.enabled = false;
	}

    void DeathHandler.HandleDeath() {
        alive = false;

        // switch to rigidbody
        controller.enabled = false;
        rigidbody.isKinematic = false;
        collider.enabled = true;
    }

    public void HandleDamage(float amount, Vector3 position, Vector3 force, DamageDealer dealer) {
        if (!alive) {
            rigidbody.AddForceAtPosition(force, position, ForceMode.Acceleration);
        }
    }
}

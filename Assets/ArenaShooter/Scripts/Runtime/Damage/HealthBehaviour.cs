using UnityEngine;
using System.Collections;
using System;

// Handle health for a gameobject, and notify handlers upon death
[AddComponentMenu("Scripts/Damage/Health Behaviour")]
public class HealthBehaviour : MonoBehaviour, DamageHandler {

    public float maxHealth;

    private bool isAlive;
    public bool IsAlive {
        get { return isAlive; }
    }

    private float currentHealth;
    public float CurrentHealth {
        get { return currentHealth; }
    }

    private DeathHandler[] handlers;

    // called upon instantiation
    void Awake() {
        handlers = GetComponentsInChildren<DeathHandler>();
    }

    // called before first update
    void Start () {
        currentHealth = maxHealth;
        isAlive = true;
    }

    // Add or remove the specified amount of health
    public void AddHealth(float amount) {
        if (!isAlive) {
            // can't add or remove health from the dead
            return;
        }

        currentHealth += amount;

        // don't go over max health
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        // die if out of health
        if (currentHealth <= 0f) {
            Die();
        }
    }

    // handle damage
    public void HandleDamage(float amount, Vector3 position, Vector3 force, DamageDealer dealer) {
        AddHealth(-amount);
    }

    // update state and notify handler
    private void Die() {
        isAlive = false;
        currentHealth = 0f;
        foreach (DeathHandler h in handlers) {
            h.HandleDeath();
        }
    }
}

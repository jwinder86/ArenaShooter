using UnityEngine;

public interface DamageHandler {
    void HandleDamage(float amount, Vector3 position, Vector3 force, DamageDealer dealer);
}

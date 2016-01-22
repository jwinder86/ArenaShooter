using UnityEngine;
using System.Collections;

public static class CustomExtensions {

    // Extension method for easily applying damage to the game object owning a component.  Returns true if collider is damageable
    // Ex: bool damageable = collider.ApplyDamage(...);
	public static bool ApplyDamage(this Component component, float amount, Vector3 position, Vector3 force, DamageDealer dealer) {
        DamageReceiver receiver = component.GetComponentInParent<DamageReceiver>();

        if (receiver == null) {
            return false;
        } else {
            receiver.ApplyDamage(amount, position, force, dealer);
            return true;
        }
    }
}

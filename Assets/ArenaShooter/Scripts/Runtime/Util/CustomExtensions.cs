using UnityEngine;
using System.Collections;

public static class CustomExtensions {

    // Extension method for easily finding the damage receiver of the game object owning a component.  Returns null if there is no DamageReceiver.
    // Ex: DamageReceiver damage = collider.GetDamageReceiver();
    public static DamageReceiver GetDamageReceiver(this Component component) {
        return (DamageReceiver)component.GetComponentInParent(typeof(DamageReceiver));
    }

    // Extension method for easily applying damage to the game object owning a component.  Returns true if collider is damageable
    // Ex: bool damageable = collider.ApplyDamage(...);
    public static bool ApplyDamage(this Component component, float amount, Vector3 position, Vector3 force, DamageDealer dealer) {
        DamageReceiver receiver = (DamageReceiver) component.GetComponentInParent(typeof(DamageReceiver));

        if (receiver == null) {
            return false;
        } else {
            receiver.ApplyDamage(amount, position, force, dealer);
            return true;
        }
    }
}

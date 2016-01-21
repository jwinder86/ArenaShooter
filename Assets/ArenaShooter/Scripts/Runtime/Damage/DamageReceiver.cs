using UnityEngine;

// Receives damage and notifies all handlers in children.  This component must be added to the root gameobject
// Since game objects may have children with colliders, this class can receive damage in one place and notify all necessary components.
// See CustomExtensions.ApplyDamage for more details
[AddComponentMenu("Scripts/Damage/Damage Receiver")]
public class DamageReceiver : MonoBehaviour {

    private DamageHandler[] handlers;

	// Use this for initialization
	public void Start () {
        // find all damage handlers
        handlers = GetComponentsInChildren<DamageHandler>();
	}

    // receive damage and notify handlers
    public void ApplyDamage(float amount, Vector3 position, Vector3 force, DamageDealer dealer) {
        if (handlers == null || handlers.Length == 0) {
            Debug.LogError("No DamageHandlers available to receive damage: " + this);
        } else {
            foreach (DamageHandler d in handlers) {
                d.HandleDamage(amount, position, force, dealer);
            }
        }
    }
}

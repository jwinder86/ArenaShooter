using UnityEngine;

// Base for reusable gameobjects
public abstract class PoolableBehaviour : MonoBehaviour {

    // private members
    private ObjectPool owner;

    // Handle activating and enabling the gameobject within the scene.  Use this instead of Start()
    abstract public void Activate();

    // Handle deactivating and disabling the gameobject within the scene.
    abstract public void Deactivate();

    // set the pool that this object should return to.
    public void SetOwner(ObjectPool pool) {
        this.owner = pool;
    }

    // deactivate object and return to pool.
    public void ReturnToPool() {
        Deactivate();

        if (owner != null) {
            owner.ReturnToPool(this);
        } else {
            Debug.LogError("No pool to return to, destroying: " + this);
            Destroy(gameObject);
        }
    }
}

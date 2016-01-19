using UnityEngine;
using System.Collections.Generic;

public class ObjectPool {

    // create an empty object in scene graph to place all runtime-created objects under
    private static Transform cachedParent;
    private static Transform parent {
        get {
            if (cachedParent == null) {
                cachedParent = (new GameObject("PoolableObjects")).transform;
            }
            return cachedParent;
        }
    }

    // settings
    private PoolableBehaviour prefab;
    private Stack<PoolableBehaviour> pool;
    private int totalCreated;
    private int incrementSize;

    public ObjectPool(PoolableBehaviour prefab, int initialSize) : this(prefab, initialSize, initialSize) { }

    public ObjectPool(PoolableBehaviour prefab, int initialSize, int incrementSize) {
        this.prefab = prefab;
        if (incrementSize < 1)
        {
            this.incrementSize = 1;
            Debug.LogError("Non-positive increment requested, defaulting to increment size of 1");
        }
        else {
            this.incrementSize = incrementSize;        
        }
        if (initialSize < 0)
        {
            initialSize = 0;
            Debug.LogError("Negative initial size requested, defaulting to size of 0");
        }

        pool = new Stack<PoolableBehaviour>(initialSize);
        totalCreated = 0;

        CreateAndAddToPool(initialSize);
    }

    // get an object from the pool, creating new objects if necessary
    public PoolableBehaviour GetFromPool() {
        if (pool.Count == 0) {
            CreateAndAddToPool(incrementSize);
            Debug.Log("Empty pool, created " + incrementSize + " new objects, " + totalCreated + " total: " + prefab);
        }

        return pool.Pop();
    }

    // return an object to this pool
    public void ReturnToPool(PoolableBehaviour obj) {
        pool.Push(obj);
        //Debug.Log("Created new object");
    }

    // create a specified number of objects and add them to this pool
    private void CreateAndAddToPool(int num) {
        for (int i = 0; i < num; i++) {
            ReturnToPool(createObject());
        }
        //Debug.Log("Created " + num + " new objects");
    }

    // create a single object and configure for this pool
    private PoolableBehaviour createObject() {
        totalCreated++;
        PoolableBehaviour created = (PoolableBehaviour)Object.Instantiate(prefab);
        created.transform.parent = parent;
        created.SetOwner(this);
        created.Deactivate();
        return created;
    }
}

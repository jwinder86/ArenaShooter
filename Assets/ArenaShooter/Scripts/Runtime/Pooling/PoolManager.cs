using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/Pooling/Pool Manager")]
public class PoolManager : MonoBehaviour {

    // cache a single PoolManager instance, create one if none exists
    // Usage: PoolManager.Instance.GetPooledObject(prefab)
    private static PoolManager cachedInstance;
    public static PoolManager Instance {
        get {
            if (cachedInstance == null) {
                PoolManager[] managers = FindObjectsOfType<PoolManager>();

                if (managers.Length == 0) {
                    Debug.LogWarning("No PoolManager in scene, creating an empty PoolManager");
                    cachedInstance = new GameObject("PoolManager").AddComponent<PoolManager>();
                } else {
                    cachedInstance = managers[0];

                    if (managers.Length > 1) {
                        Debug.LogError("More than one PoolManager in scene, selecting first.");
                    }
                }
            }

            return cachedInstance;
        }
    }

    private Dictionary<PoolableBehaviour, ObjectPool> poolMap;

    public int defaultPoolSize = 10;

    //TODO: create custom editor for easily configuring pool sizes
    public PoolableBehaviour[] poolableObjects;
    public int[] initialSizes;

    void Start() {
        if (poolMap == null) {
            InitializePools();
        }
    }

    // set up initial pooling as configured in editor
    private void InitializePools() {
        if (poolableObjects == null || initialSizes == null || (poolableObjects.Length == 0 && initialSizes.Length == 0)) {
            Debug.LogWarning("No object pooling configured, pools will be created as requested using initialSize of: " + defaultPoolSize);
        } else if (poolableObjects.Length != initialSizes.Length) {
            Debug.LogError("Poolable Objects (" + poolableObjects.Length + ") doesn't match Initial Sizes (" + initialSizes.Length + ").  Failed to initialize pools.");
            poolMap = new Dictionary<PoolableBehaviour, ObjectPool>();
        } else {
            poolMap = new Dictionary<PoolableBehaviour, ObjectPool>(poolableObjects.Length);
            for (int i = 0; i < poolableObjects.Length; i++) {
                poolMap[poolableObjects[i]] = new ObjectPool(poolableObjects[i], initialSizes[i]);
            }
        }
    }

    // Get a pooled instance of the specified prefab, creating pool or instances as necessary
    public PoolableBehaviour GetPooledObject(PoolableBehaviour prefab) {
        if (poolMap == null) {
            InitializePools();
        }

        if (!poolMap.ContainsKey(prefab)) {
            poolMap[prefab] = new ObjectPool(prefab, defaultPoolSize);
            Debug.LogWarning("Creating pool for uninitialized prefab: " + prefab + " with initial size of " + defaultPoolSize);
        }

        return poolMap[prefab].GetFromPool();
    }
}

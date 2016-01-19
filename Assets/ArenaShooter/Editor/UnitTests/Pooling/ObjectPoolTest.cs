using UnityEngine;
using System.Collections;
using NUnit.Framework;

[TestFixture]
[Category("Pooling")]
public class ObjectPoolTest {

    [Test]
    public void testEmptyPool() {

        GameObject obj = new GameObject();
        PoolableBehaviour prefab = obj.AddComponent<TestablePoolableBehaviour>();
        int initialSize=0;
        int incrementSize=1;
        ObjectPool pool = new ObjectPool(prefab, initialSize, incrementSize);

        PoolableBehaviour retrieved = pool.GetFromPool();

        Assert.IsNotNull(retrieved);
    }

    [Test]
    public void testEmptyPoolInvalidIncrement()
    {

        GameObject obj = new GameObject();
        PoolableBehaviour prefab = obj.AddComponent<TestablePoolableBehaviour>();
        int initialSize = 0;
        int incrementSize = -1;
        ObjectPool pool = new ObjectPool(prefab, initialSize, incrementSize);

        PoolableBehaviour retrieved = pool.GetFromPool();

        Assert.IsNotNull(retrieved);
    }

    [Test]
    public void testInvalidInitialSize()
    {

        GameObject obj = new GameObject();
        PoolableBehaviour prefab = obj.AddComponent<TestablePoolableBehaviour>();
        int initialSize = -10;
        int incrementSize = 1;
        ObjectPool pool = new ObjectPool(prefab, initialSize, incrementSize);

        PoolableBehaviour retrieved = pool.GetFromPool();

        Assert.IsNotNull(retrieved);
    }

    
    [Test]
    public void testInitializeLargePool()
    {

        GameObject obj = new GameObject();
        PoolableBehaviour prefab = obj.AddComponent<TestablePoolableBehaviour>();
        int initialSize = 100;
        int incrementSize = 1;
        ObjectPool pool = new ObjectPool(prefab, initialSize, incrementSize);

        PoolableBehaviour retrieved = pool.GetFromPool();

        Assert.IsNotNull(retrieved);
    }
}



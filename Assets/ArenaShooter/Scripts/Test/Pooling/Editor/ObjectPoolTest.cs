using UnityEngine;
using System.Collections;
using NUnit.Framework;

namespace UnitTests {

    [TestFixture]
    [Category("Pooling")]
    public class ObjectPoolTest {

        [Test]
        public void testEmptyPool() {

            PoolableBehaviour prefab = RuntimeTestHelpers.CreatePrefabWithComponent<TestablePoolableBehaviour>("Test Poolable");
            int initialSize = 0;
            int incrementSize = 1;
            ObjectPool pool = new ObjectPool(prefab, initialSize, incrementSize);

            PoolableBehaviour retrieved = pool.GetFromPool();

            Assert.IsNotNull(retrieved);
        }

        [Test]
        public void testEmptyPoolInvalidIncrement() {

            PoolableBehaviour prefab = RuntimeTestHelpers.CreatePrefabWithComponent<TestablePoolableBehaviour>("Test Poolable");
            int initialSize = 0;
            int incrementSize = -1;
            ObjectPool pool = new ObjectPool(prefab, initialSize, incrementSize);

            PoolableBehaviour retrieved = pool.GetFromPool();

            Assert.IsNotNull(retrieved);
        }

        [Test]
        public void testInvalidInitialSize() {

            PoolableBehaviour prefab = RuntimeTestHelpers.CreatePrefabWithComponent<TestablePoolableBehaviour>("Test Poolable");
            int initialSize = -10;
            int incrementSize = 1;
            ObjectPool pool = new ObjectPool(prefab, initialSize, incrementSize);

            PoolableBehaviour retrieved = pool.GetFromPool();

            Assert.IsNotNull(retrieved);
        }


        [Test]
        public void testInitializeLargePool() {

            PoolableBehaviour prefab = RuntimeTestHelpers.CreatePrefabWithComponent<TestablePoolableBehaviour>("Test Poolable");
            int initialSize = 100;
            int incrementSize = 1;
            ObjectPool pool = new ObjectPool(prefab, initialSize, incrementSize);

            PoolableBehaviour retrieved = pool.GetFromPool();

            Assert.IsNotNull(retrieved);
        }

        [Test]
        public void testPoolReuse() {
            PoolableBehaviour prefab = RuntimeTestHelpers.CreatePrefabWithComponent<TestablePoolableBehaviour>("Test Poolable");
            ObjectPool pool = new ObjectPool(prefab, 1, 1);

            // pool should never return the prefab itself
            PoolableBehaviour retrieved = pool.GetFromPool();
            Assert.AreNotEqual(prefab, retrieved, "Prefab itself returned from pool");

            // pool should not return an in-use object
            PoolableBehaviour retrieved2 = pool.GetFromPool();
            Assert.AreNotEqual(retrieved, retrieved2, "Pool returned in-use object twice");

            // object returned to pool should be retrieved from pool
            retrieved.ReturnToPool();
            PoolableBehaviour retrieved3 = pool.GetFromPool();
            Assert.AreEqual(retrieved, retrieved3, "Pool did not re-use returned object");
        }

        [Test]
        public void testAutoPoolManagerCreation() {
            PoolableBehaviour prefab = RuntimeTestHelpers.CreatePrefabWithComponent<TestablePoolableBehaviour>("Test Poolable");

            // request object without pre-creating PoolManager
            PoolableBehaviour obj = PoolManager.Instance.GetPooledObject(prefab);

            Assert.NotNull(obj, "Retrieved null object from pool");
            Assert.AreNotEqual(prefab, obj, "Received prefab itself from pool");
        }
    }
}



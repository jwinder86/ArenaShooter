using UnityEngine;
using System.Collections;
using NUnit.Framework;

namespace UnitTests {

    [TestFixture]
    [Category("Pooling")]
    public class ObjectPoolTest {

        [Test]
        public void testEmptyPool() {

            PoolableBehaviour prefab = TestHelpers.CreatePrefabWithComponent<TestablePoolableBehaviour>("PoolableDummy");
            int initialSize = 0;
            int incrementSize = 1;
            ObjectPool pool = new ObjectPool(prefab, initialSize, incrementSize);

            PoolableBehaviour retrieved = pool.GetFromPool();

            Assert.IsNotNull(retrieved);
        }

        [Test]
        public void testEmptyPoolInvalidIncrement() {

            PoolableBehaviour prefab = TestHelpers.CreatePrefabWithComponent<TestablePoolableBehaviour>("PoolableDummy");
            int initialSize = 0;
            int incrementSize = -1;
            ObjectPool pool = new ObjectPool(prefab, initialSize, incrementSize);

            PoolableBehaviour retrieved = pool.GetFromPool();

            Assert.IsNotNull(retrieved);
        }

        [Test]
        public void testInvalidInitialSize() {

            PoolableBehaviour prefab = TestHelpers.CreatePrefabWithComponent<TestablePoolableBehaviour>("PoolableDummy");
            int initialSize = -10;
            int incrementSize = 1;
            ObjectPool pool = new ObjectPool(prefab, initialSize, incrementSize);

            PoolableBehaviour retrieved = pool.GetFromPool();

            Assert.IsNotNull(retrieved);
        }


        [Test]
        public void testInitializeLargePool() {

            PoolableBehaviour prefab = TestHelpers.CreatePrefabWithComponent<TestablePoolableBehaviour>("PoolableDummy");
            int initialSize = 100;
            int incrementSize = 1;
            ObjectPool pool = new ObjectPool(prefab, initialSize, incrementSize);

            PoolableBehaviour retrieved = pool.GetFromPool();

            Assert.IsNotNull(retrieved);
        }

        [Test]
        public void testPoolReuse() {
            PoolableBehaviour prefab = TestHelpers.CreatePrefabWithComponent<TestablePoolableBehaviour>("PoolableDummy");
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
    }
}



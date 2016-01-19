using UnityEngine;
using System.Collections;
using NUnit.Framework;

[TestFixture]
[Category("Pooling")]
public class ObjectPoolTest {

    [Test]
    public void sampleTest() {

        PoolableBehaviour prefab = new TestablePoolableBehaviour();
        int initialSize=0;
        int incrementSize=1;
        ObjectPool pool = new ObjectPool(prefab, initialSize, incrementSize);

        PoolableBehaviour retrieved = pool.GetFromPool();

        Assert.IsNotNull(retrieved);


    }
}



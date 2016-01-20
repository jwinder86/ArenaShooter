using UnityEngine;
using System.Collections;

// prevents code from being compiled for stand-alone builds
#if UNITY_EDITOR

namespace UnitTests {

    // place under "Test" in Add Component menu to prevent cluttering
    [AddComponentMenu("Test/Pooling/TestablePoolableBehaviour")]
    public class TestablePoolableBehaviour : PoolableBehaviour {

        // Handle activating and enabling the gameobject within the scene.  Use this instead of Start()
        public override void Activate() {
            // do nothing
        }

        // Handle deactivating and disabling the gameobject within the scene.
        public override void Deactivate() {
            // do nothing
        }
    }
}

#endif

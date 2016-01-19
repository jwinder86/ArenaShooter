using UnityEngine;
using System.Collections;

public class TestablePoolableBehaviour : PoolableBehaviour {

    // Handle activating and enabling the gameobject within the scene.  Use this instead of Start()
    public override void Activate() {
        //this.Activate();
    }

    // Handle deactivating and disabling the gameobject within the scene.
    public override void Deactivate() {
        //this.Deactivate();
    }
}

using UnityEngine;
using System.Collections;
using System;

// prevents code from being compiled for stand-alone builds
#if UNITY_EDITOR

namespace UnitTests {

    // place under "Test" in Add Component menu to prevent cluttering
    [AddComponentMenu("Test/Damage/MockDamageHandlerBehaviour")]
    public class MockDamageHandlerBehaviour : MonoBehaviour, DamageHandler {

        public bool wasDamaged = false;

        public void HandleDamage(float amount, Vector3 position, Vector3 force, DamageDealer dealer) {
            wasDamaged = true;
        }
    }
}

#endif
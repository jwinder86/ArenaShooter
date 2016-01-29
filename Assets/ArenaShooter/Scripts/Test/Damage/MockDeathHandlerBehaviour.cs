using UnityEngine;
using System.Collections;
using System;

// prevents code from being compiled for stand-alone builds
#if UNITY_EDITOR

namespace UnitTests {

    // place under "Test" in Add Component menu to prevent cluttering
    [AddComponentMenu("Test/Damage/MockDamageHandlerBehaviour")]
    public class MockDeathHandlerBehaviour : MonoBehaviour, DeathHandler {

        public bool isDead = false;

        public void HandleDeath() {
            isDead = true;
        }
    }
}

#endif

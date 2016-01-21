using UnityEngine;
using System.Collections;

#if UNITY_EDITOR

namespace UnitTests {
    public static class TestHelpers {

        // Create a game object with specified component
	    public static T CreateObjectWithComponent<T>() where T : Component {
            GameObject obj = new GameObject();
            T component = obj.AddComponent<T>();
            return component;
        }

        // Create prefab object (prefabs are never active)
        public static T CreatePrefabWithComponent<T>() where T : Component {
            T component = CreateObjectWithComponent<T>();
            component.gameObject.SetActive(false);
            return component;
        }

        // Helper to auto-reference GameObject from MonoBehaviour
        public static T AddComponent<T>(this Component obj) where T : Component {
            return obj.gameObject.AddComponent<T>();
        }

        // Add a child to a GameObject
        public static void AddChild(this Component parent, Component child) {
            child.transform.parent = parent.transform;
        }
    }
}

#endif
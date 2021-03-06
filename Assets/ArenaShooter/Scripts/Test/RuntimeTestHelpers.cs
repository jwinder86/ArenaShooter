﻿using UnityEngine;
using System.Collections;
using System.Reflection;

#if UNITY_EDITOR

namespace UnitTests {

    // Unit Test helpers which are compiled in the Runtime assembly.  These can perform runtime operations, such as instantiating GameObjects.
    public static class RuntimeTestHelpers {

        // Create a game object with specified component, call Awake if possible
        public static T CreateObjectWithComponent<T>(string name = "New Test Object") where T : Component {
            GameObject obj = new GameObject(name);
            T component = obj.AddComponent<T>();

            return component;
        }

        // Create prefab object (prefabs are never active, don't call Awake)
        public static T CreatePrefabWithComponent<T>(string name = "New Test Prefab") where T : Component {
            T component = CreateObjectWithComponent<T>(name);
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

        // Call Awake() on all components under the game object
        public static void CallAwakeAndStartRecursive(this Component obj) {
            // call Awake then Start
            obj.CallPrivateMethodRecursive("Awake");
            obj.CallPrivateMethodRecursive("Start");
        }

        // call the same private method on all components under a gameobject
        private static void CallPrivateMethodRecursive(this Component component, string methodName) {
            MonoBehaviour[] behaviours = component.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour behaviour in behaviours) {
                behaviour.ComponentPrivateMethod(methodName);
            }
        }

        // call a private method on a specific component
        private static void ComponentPrivateMethod<T>(this T component, string methodName) where T : Component {
            MethodInfo method = component.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null) {
                Debug.Log("Called non-public " + methodName + "() on '" + component + "'");
                method.Invoke(component, null);
            }
        }

        // Call a private method
        public static object CallPrivateMethod(this object script, string methodName, params object[] parameters) {
            MethodInfo method = script.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null) {
                return method.Invoke(script, parameters);
            } else {
                Debug.LogError("No private method \"" + methodName + "\" in script: " + script.GetType());
                return null;
            }
        }

        // Call a private static method
        public static object CallPrivateMethod(this System.Type type, string methodName, params object[] parameters) {
            MethodInfo method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            if (method != null) {
                return method.Invoke(null, parameters);
            } else {
                Debug.LogError("No private method \"" + methodName + "\" in script: " + type);
                return null;
            }
        }

        // Determine if two vectors are equal, considering a delta
        public static bool EqualsDelta(this Vector3 vector, Vector3 other, float delta) {
            return (vector - other).magnitude < delta;
        }
    }
}

#endif
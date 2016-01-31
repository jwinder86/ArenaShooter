using UnityEngine;
using UnityEditor;
using NUnit.Framework;

namespace UnitTests {

    // compares if two vectors are equal, given a delta
    public class VectorAssert {
        public static void AreEqual(Vector3 expected, Vector3 actual, float delta, string message = null) {
            float actualDelta = Delta(expected, actual);

            if (actualDelta > delta) {
                string output = " Expected: " + expected + " with delta magnitude up to: " + delta + "\n" +
                    " But was: " + actual + " with delta magnitude of: " + actualDelta;

                if (string.IsNullOrEmpty(message)) {
                    Assert.Fail(output);
                } else {
                    Assert.Fail(message + "\n" + output);
                }
            }
        }

        public static void AreNotEqual(Vector3 expected, Vector3 actual, float delta, string message = null) {
            float actualDelta = Delta(expected, actual);

            if (actualDelta <= delta) {
                string output = " Expected: " + expected + " with delta magnitude greater than: " + delta + "\n" +
                    " But was: " + actual + " with delta magnitude of: " + actualDelta;

                if (string.IsNullOrEmpty(message)) {
                    Assert.Fail(output);
                } else {
                    Assert.Fail(message + "\n" + output);
                }
            }
        }

        private static float Delta(Vector3 expected, Vector3 actual) {
            return (expected - actual).magnitude;
        }
    }
}
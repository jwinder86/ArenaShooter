using UnityEngine;
using System.Collections;
using NUnit.Framework;

namespace UnitTests {

    [TestFixture]
    [Category ("Player")]
    public class CameraAimTest {

        [Test]
        public void testCameraFade() {
            ensureCameraExists();

            CameraAimBehaviour camera = RuntimeTestHelpers.CreateObjectWithComponent<CameraAimBehaviour>("Camera Aim");
            MeshRenderer renderer = camera.AddComponent<MeshRenderer>();
            
            renderer.sharedMaterial = new Material(Shader.Find("Standard"));
            camera.charRenderer = renderer;
            camera.transparencyMaterial = new Material(Shader.Find("Standard"));

            camera.CallAwakeAndStartRecursive();

            camera.minFadeDistance = 1f;
            camera.maxFadeDistance = 2f;

            // move past max distance
            camera.CallPrivateMethod("FadeCameraDistance", 3f);

            Assert.IsTrue(renderer.enabled, "Renderer not displayed");
            Assert.AreNotEqual(renderer.sharedMaterial, camera.transparencyMaterial, "Using transparent material when too far away.");

            // move to midpoint
            camera.CallPrivateMethod("FadeCameraDistance", 1.5f);
            Assert.IsTrue(renderer.enabled, "Renderer not displayed");
            Assert.AreEqual(renderer.sharedMaterial, camera.transparencyMaterial, "Not using transparent material when in fade range");
            Assert.AreEqual(camera.transparencyMaterial.color.a, 0.5f, 0.01f, "Expected 50% opacity in middle of fade range");

            // move up before min distance
            camera.CallPrivateMethod("FadeCameraDistance", 0.5f);
            Assert.IsFalse(renderer.enabled, "Renderer displayed when not expected");

            // reset past max distance
            camera.CallPrivateMethod("FadeCameraDistance", 3f);

            Assert.IsTrue(renderer.enabled, "Renderer not displayed");
            Assert.AreNotEqual(renderer.sharedMaterial, camera.transparencyMaterial, "Using transparent material when too far away.");
        }

        private void ensureCameraExists() {
            if (Camera.main == null) {
                Camera camera = RuntimeTestHelpers.CreateObjectWithComponent<Camera>("Main Camera");
                camera.tag = "MainCamera";
            }
        }
    }
}

using UnityEngine;
using NUnit.Framework;

namespace UnitTests {

    [TestFixture]
    [Category("Damage")]
    public class DamageReceiverTest {

        [Test]
        public void SingleDamageHandlerTest() {

            DamageReceiver receiver = RuntimeTestHelpers.CreateObjectWithComponent<DamageReceiver>("Damage Receiver");
            MockDamageHandlerBehaviour handler = receiver.AddComponent<MockDamageHandlerBehaviour>();

            receiver.CallAwakeAndStartRecursive();

            Assert.False(handler.wasDamaged, "Handler damaged before test conditions.");

            receiver.ApplyDamage(1f, Vector3.zero, Vector3.zero, DamageDealer.Default);

            Assert.True(handler.wasDamaged, "Handler not damaged.");
        }

        [Test]
        public void MultipleDamageHandlerTest() {
            DamageReceiver receiver = RuntimeTestHelpers.CreateObjectWithComponent<DamageReceiver>("Damage Receiver");
            MockDamageHandlerBehaviour handler1 = receiver.AddComponent<MockDamageHandlerBehaviour>();
            MockDamageHandlerBehaviour handler2 = receiver.AddComponent<MockDamageHandlerBehaviour>();

            receiver.CallAwakeAndStartRecursive();

            Assert.False(handler1.wasDamaged, "Handler1 damaged before test conditions.");
            Assert.False(handler2.wasDamaged, "Handler2 damaged before test conditions.");

            receiver.ApplyDamage(1f, Vector3.zero, Vector3.zero, DamageDealer.Default);

            Assert.True(handler1.wasDamaged, "Handler1 not damaged.");
            Assert.True(handler2.wasDamaged, "Handler2 not damaged.");
        }

        [Test]
        public void NestedSingleDamageHandlerTest() {
            DamageReceiver receiver = RuntimeTestHelpers.CreateObjectWithComponent<DamageReceiver>("Damage Receiver");
            MockDamageHandlerBehaviour handler = RuntimeTestHelpers.CreateObjectWithComponent<MockDamageHandlerBehaviour>();
            receiver.AddChild(handler);

            receiver.CallAwakeAndStartRecursive();

            Assert.False(handler.wasDamaged, "Handler damaged before test conditions.");

            receiver.ApplyDamage(1f, Vector3.zero, Vector3.zero, DamageDealer.Default);

            Assert.True(handler.wasDamaged, "Handler not damaged.");
        }

        [Test]
        public void NestedMultipleDamageHandlerTest() {
            DamageReceiver receiver = RuntimeTestHelpers.CreateObjectWithComponent<DamageReceiver>("Damage Receiver");
            MockDamageHandlerBehaviour handler1 = RuntimeTestHelpers.CreateObjectWithComponent<MockDamageHandlerBehaviour>();
            MockDamageHandlerBehaviour handler2 = RuntimeTestHelpers.CreateObjectWithComponent<MockDamageHandlerBehaviour>();
            receiver.AddChild(handler1);
            handler1.AddChild(handler2);

            receiver.CallAwakeAndStartRecursive();

            Assert.False(handler1.wasDamaged, "Handler1 damaged before test conditions.");
            Assert.False(handler2.wasDamaged, "Handler2 damaged before test conditions.");

            receiver.ApplyDamage(1f, Vector3.zero, Vector3.zero, DamageDealer.Default);

            Assert.True(handler1.wasDamaged, "Handler1 not damaged.");
            Assert.True(handler2.wasDamaged, "Handler2 not damaged.");
        }

        [Test]
        public void ColliderDamagerHandlerTest() {
            DamageReceiver receiver = RuntimeTestHelpers.CreateObjectWithComponent<DamageReceiver>("Damage Receiver");
            MockDamageHandlerBehaviour handler = receiver.AddComponent<MockDamageHandlerBehaviour>();
            Collider collider = receiver.AddComponent<SphereCollider>();

            receiver.CallAwakeAndStartRecursive();

            Assert.False(handler.wasDamaged, "Handler damaged before test conditions.");

            collider.ApplyDamage(1f, Vector3.zero, Vector3.zero, DamageDealer.Default);

            Assert.True(handler.wasDamaged, "Handler not damaged.");
        }

        [Test]
        public void NestedColliderDamagerHandlerTest() {
            DamageReceiver receiver = RuntimeTestHelpers.CreateObjectWithComponent<DamageReceiver>("Damage Receiver");
            MockDamageHandlerBehaviour handler = RuntimeTestHelpers.CreateObjectWithComponent<MockDamageHandlerBehaviour>();
            Collider collider = RuntimeTestHelpers.CreateObjectWithComponent<SphereCollider>();
            receiver.AddChild(handler);
            receiver.AddChild(collider);

            receiver.CallAwakeAndStartRecursive();

            Assert.False(handler.wasDamaged, "Handler damaged before test conditions.");

            collider.ApplyDamage(1f, Vector3.zero, Vector3.zero, DamageDealer.Default);

            Assert.True(handler.wasDamaged, "Handler not damaged.");
        }
    }
}
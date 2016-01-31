using UnityEngine;
using System.Collections;
using NUnit.Framework;

namespace UnitTests {

    [TestFixture]
    [Category("Damage")]
    public class HealthTest {

        [Test]
        public void TestHealthRemoved() {
            DamageReceiver receiver = RuntimeTestHelpers.CreateObjectWithComponent<DamageReceiver>("Health Test");
            HealthBehaviour health = receiver.AddComponent<HealthBehaviour>();

            health.maxHealth = 1f;

            receiver.CallAwakeAndStartRecursive();

            Assert.True(health.IsAlive, "Dead before taking damage");
            Assert.AreEqual(1f, health.CurrentHealth, 0.001f, "Health doesn't match specified value");

            receiver.ApplyDamage(0.75f, Vector3.zero, Vector3.zero, DamageDealer.Default);

            Assert.True(health.IsAlive, "Dead before all health removed");
            Assert.AreEqual(0.25f, health.CurrentHealth, 0.001f, "Health doesn't match specified value minus removed amount");

            receiver.ApplyDamage(0.25f, Vector3.zero, Vector3.zero, DamageDealer.Default);

            Assert.False(health.IsAlive, "Still alive after all health removed");
            Assert.AreEqual(0f, health.CurrentHealth, 0.001f, "Still health remaining after dead");
        }

        [Test]
        public void TestDeathHandler() {
            DamageReceiver receiver = RuntimeTestHelpers.CreateObjectWithComponent<DamageReceiver>("Health Test");
            HealthBehaviour health = receiver.AddComponent<HealthBehaviour>();
            MockDeathHandlerBehaviour handler = receiver.AddComponent<MockDeathHandlerBehaviour>();

            health.maxHealth = 1f;

            receiver.CallAwakeAndStartRecursive();

            Assert.False(handler.isDead, "Handler dead before taking damage");

            receiver.ApplyDamage(1f, Vector3.zero, Vector3.zero, DamageDealer.Default);

            Assert.True(handler.isDead, "Handler still alive after taking damage");
        }
    }
}
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

namespace UnitTests {

    [TestFixture]
    [Category ("Weapon")]
    public class WeaponConfigTest {

        [Test]
        public void testConfigValidationNoPrefab() {
            WeaponConfig config = new WeaponConfig();
            config.projectilePrefab = null;
            config.fireInterval = 1f;
            config.sprayAngle = 1f;

            Assert.IsFalse(config.Validate(), "Valid despite null projectile");
        }

        [Test]
        public void testConfigValidationBadFireInterval() {
            WeaponConfig config = new WeaponConfig();
            config.projectilePrefab = RuntimeTestHelpers.CreatePrefabWithComponent<ProjectileBehaviour>();
            config.fireInterval = 0f;
            config.sprayAngle = 1f;

            Assert.IsFalse(config.Validate(), "Valid despite zero fire interval");

            config.fireInterval = -1f;

            Assert.IsFalse(config.Validate(), "Valid despite negative fire interval");
        }

        [Test]
        public void testConfigValidationBadSprayAngle() {
            WeaponConfig config = new WeaponConfig();
            config.projectilePrefab = RuntimeTestHelpers.CreatePrefabWithComponent<ProjectileBehaviour>();
            config.fireInterval = 1f;
            config.sprayAngle = -1f;

            Assert.IsFalse(config.Validate(), "Valid despite negative spray angle");
        }
    }
}

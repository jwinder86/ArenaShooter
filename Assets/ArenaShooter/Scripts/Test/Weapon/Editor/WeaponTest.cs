using UnityEngine;
using UnityEditor;
using NUnit.Framework;

namespace UnitTests {

    [TestFixture]
    [Category ("Weapon")]
    public class WeaponTest {

        [Test]
        public void testFireDelay() {
            WeaponConfig config = new WeaponConfig();
            config.projectilePrefab = TestHelpers.CreatePrefabWithComponent<ProjectileBehaviour>();
            config.fireInterval = 1f;
            config.sprayAngle = 0f;

            Weapon weapon = new Weapon(config);

            // can fire immediately
            Assert.IsTrue(weapon.CanFire, "Weapon cannot fire upon creation");
            
            // fire weapon
            Assert.IsTrue(weapon.FireWeapon(Vector3.zero, Vector3.up, DamageDealer.Default), "Weapon didn't fire");

            // can't re-fire immediately
            Assert.IsFalse(weapon.CanFire, "Weapon can fire without any delay");
            Assert.IsFalse(weapon.FireWeapon(Vector3.zero, Vector3.up, DamageDealer.Default), "Weapon fired without any delay");

            // wait part of delay time
            weapon.UpdateTimer(0.25f);

            // can't re-fire after partial delay
            Assert.IsFalse(weapon.CanFire, "Weapon can fire without full delay");
            Assert.IsFalse(weapon.FireWeapon(Vector3.zero, Vector3.up, DamageDealer.Default), "Weapon fired without full delay");

            // wait rest of delay time
            weapon.UpdateTimer(0.75f);

            // weapon fires again after rest of delay
            Assert.IsTrue(weapon.CanFire, "Weapon cannot fire after delay");
            Assert.IsTrue(weapon.FireWeapon(Vector3.zero, Vector3.up, DamageDealer.Default), "Weapon didn't fire after delay");
        }

        [Test]
        public void testSprayAngle() {
            WeaponConfig config = new WeaponConfig();
            config.projectilePrefab = TestHelpers.CreatePrefabWithComponent<ProjectileBehaviour>();
            config.fireInterval = 1f;
            config.sprayAngle = 10f;

            Weapon weapon = new Weapon(config);

            // randomly test 10,000 random angles
            // not deterministic, but also difficult to test deterministically without deeper understanding of Quaternion implementation
            for (int i = 0; i < 10000; i++) {
                testAngle(Random.onUnitSphere, Random.Range(1f, 90f));
            }
        }

        // test if an angle adjustment is within specified max
        private void testAngle(Vector3 inputAngle, float maxAngle) {
            Vector3 newAngle = (Vector3)typeof(Weapon).CallPrivateMethod("RandomDirectionOffset", inputAngle, maxAngle);
            float diff = Vector3.Angle(inputAngle, newAngle);
            Assert.LessOrEqual(diff, maxAngle, "Randomized direction (" + newAngle + ") differs from input direction (" + inputAngle + ") by more than " + maxAngle + " degrees (" + diff + ")");
        }
    }
}

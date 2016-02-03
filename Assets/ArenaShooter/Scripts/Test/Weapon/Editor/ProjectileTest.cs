using UnityEngine;
using UnityEditor;
using NUnit.Framework;

namespace UnitTests {

    [TestFixture]
    [Category ("Weapon")]
    public class ProjectileTest {

        private const float DELTA = 0.0001f;

        [Test]
        public void testTransformFollowPhysics() {
            ProjectileBehaviour projectile = RuntimeTestHelpers.CreateObjectWithComponent<ProjectileBehaviour>();
            projectile.speed = 1f;
            projectile.lifetime = 10f;
            projectile.damage = 1f;
            projectile.force = 1f;

            // start projectile
            projectile.InitializeAndActivate(Vector3.zero, Vector3.forward, DamageDealer.Default);

            // test no movement
            VectorAssert.AreEqual(Vector3.zero, projectile.PhysicsPosition, DELTA, "Physics position not zero");
            VectorAssert.AreEqual(Vector3.zero, projectile.transform.position, DELTA, "Transform position not zero");

            // move physics position forward 2s
            projectile.CallPrivateMethod("PhysicsUpdate", 2f);

            // test physics moved but not game object
            VectorAssert.AreEqual(Vector3.forward * 2f, projectile.PhysicsPosition, DELTA, "Physics position incorrect");
            VectorAssert.AreEqual(Vector3.zero, projectile.transform.position, DELTA, "Transform position not zero");

            // move transform position by half step
            projectile.CallPrivateMethod("MoveToRaycastGoal", 1f);

            VectorAssert.AreEqual(Vector3.forward * 2f, projectile.PhysicsPosition, DELTA, "Physics position incorrect");
            VectorAssert.AreEqual(Vector3.forward, projectile.transform.position, DELTA, "Transform position not half of physics position");

            // move transform position by another half step
            projectile.CallPrivateMethod("MoveToRaycastGoal", 1f);

            VectorAssert.AreEqual(Vector3.forward * 2f, projectile.PhysicsPosition, DELTA, "Physics position incorrect");
            VectorAssert.AreEqual(projectile.PhysicsPosition, projectile.transform.position, DELTA, "Transform position does not match physics position");

            // transform position shouldn't move without physics position moving
            projectile.CallPrivateMethod("MoveToRaycastGoal", 5f);

            VectorAssert.AreEqual(Vector3.forward * 2f, projectile.PhysicsPosition, DELTA, "Physics position incorrect");
            VectorAssert.AreEqual(projectile.PhysicsPosition, projectile.transform.position, DELTA, "Transform position does not match physics position");
        }

        [Test]
        public void testProjectileDuration() {
            ProjectileBehaviour projectile = RuntimeTestHelpers.CreateObjectWithComponent<ProjectileBehaviour>();
            projectile.speed = 1f;
            projectile.lifetime = 1f;
            projectile.damage = 1f;
            projectile.force = 1f;

            // start projectile
            projectile.InitializeAndActivate(Vector3.zero, Vector3.forward, DamageDealer.Default);

            Assert.IsTrue(projectile.IsAlive, "Projectile not alive after activation");

            // still alive after .5s
            projectile.CallPrivateMethod("UpdateLifeTimer", 0.5f);
            Assert.IsTrue(projectile.IsAlive, "Projectile not alive after .5s");

            // not alive after 1s
            projectile.CallPrivateMethod("UpdateLifeTimer", 0.5f);
            Assert.IsFalse(projectile.IsAlive, "Projectile alive after lifetime");

            // projectile hasn't moved without calling update methods
            VectorAssert.AreEqual(Vector3.zero, projectile.PhysicsPosition, DELTA, "Physics position moved");
            VectorAssert.AreEqual(Vector3.zero, projectile.transform.position, DELTA, "Transform position moved");

            // physics can't move while not alive
            projectile.CallPrivateMethod("PhysicsUpdate", 1f);
            VectorAssert.AreEqual(Vector3.zero, projectile.PhysicsPosition, DELTA, "Physics position moved");
            projectile.CallPrivateMethod("MoveToRaycastGoal", 1f);
            VectorAssert.AreEqual(Vector3.zero, projectile.transform.position, DELTA, "Transform position moved");
        }

        [Test]
        public void testProjectileTransformMoveNotAlive() {
            ProjectileBehaviour projectile = RuntimeTestHelpers.CreateObjectWithComponent<ProjectileBehaviour>();
            projectile.speed = 1f;
            projectile.lifetime = 1f;
            projectile.damage = 1f;
            projectile.force = 1f;

            // start projectile
            projectile.InitializeAndActivate(Vector3.zero, Vector3.forward, DamageDealer.Default);

            projectile.CallPrivateMethod("PhysicsUpdate", 1f);
            projectile.CallPrivateMethod("UpdateLifeTimer", 1f);

            // assert projectile not alive and transform not updated
            VectorAssert.AreEqual(Vector3.forward, projectile.PhysicsPosition, DELTA, "Physics position not correct");
            Assert.IsFalse(projectile.IsAlive, "Projectile alive after lifetime");
            VectorAssert.AreEqual(Vector3.zero, projectile.transform.position, DELTA, "Transform position not correct");

            // transform moves to final position regardless of time
            projectile.CallPrivateMethod("MoveToRaycastGoal", 0.01f);
            VectorAssert.AreEqual(Vector3.forward, projectile.transform.position, DELTA, "Transform did not move to final position");
        }
    }
}

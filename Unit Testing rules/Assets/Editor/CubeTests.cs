using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CubeTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(-29)]
        public void DisableOnDeath_EmptyHP_ObjectSetInactive(float hp)
        {
            GameObject testObject = MakeFakeCube(hp);

            Assert.IsFalse(testObject.activeSelf);
        }

        [Test]
        [TestCase(1)]
        [TestCase(100)]
        [TestCase(0.6f)]
        [TestCase(9999999)]
        public void DisableOnDeath_HasHP_ObjectRemainsActive(float hp)
        {
            GameObject testObject = MakeFakeCube(hp);

            Assert.IsTrue(testObject.activeSelf);
        }

        private static GameObject MakeFakeCube(float hp)
        {
            GameObject testObject = new GameObject();
            Cube cubeScript = testObject.AddComponent<Cube>();

            cubeScript.health = hp;
            cubeScript.DisableOnDeath();
            return testObject;
        }
    }
}

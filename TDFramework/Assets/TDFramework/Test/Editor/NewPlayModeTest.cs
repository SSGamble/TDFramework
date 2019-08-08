using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace TDFramework {
    public class NewPlayModeTest {

        class MonoSingletonTestClass : MonoSingleton<MonoSingletonTestClass> {
        }

        [UnityTest]
        public IEnumerator MonoSingletonTest() {
            var instanceA = MonoSingletonTestClass.Instance;
            var instanceB = MonoSingletonTestClass.Instance;
            Assert.AreEqual(instanceA.GetHashCode(), instanceB.GetHashCode());
            yield return null;
        }
    }
}

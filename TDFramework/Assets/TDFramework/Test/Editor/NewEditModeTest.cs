/****************************************************
	文件：NewEditModeTest.cs
	作者：CaptainYun
	日期：2019/08/07 18:40   	
	功能：单元测试：单例模板
*****************************************************/
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace TDFramework {
    public class NewEditModeTest {

        class SingletonTestClass : Singleton<SingletonTestClass> {
            private SingletonTestClass() { }
        }

        [Test]
        public void SingletonTest() {
            var instanceA = SingletonTestClass.Instance;
            var instanceB = SingletonTestClass.Instance;
            Assert.AreEqual(instanceA.GetHashCode(), instanceB.GetHashCode());
        }
    }
}

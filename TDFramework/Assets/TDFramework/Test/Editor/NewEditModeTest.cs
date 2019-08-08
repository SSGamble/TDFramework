/****************************************************
	�ļ���NewEditModeTest.cs
	���ߣ�CaptainYun
	���ڣ�2019/08/07 18:40   	
	���ܣ���Ԫ���ԣ�����ģ��
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

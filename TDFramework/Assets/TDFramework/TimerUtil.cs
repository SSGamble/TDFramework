/****************************************************
    文件：TimerUtil.cs
	作者：TravelerTD
    日期：2019/8/6 17:35:41
	功能：通过协程实现的定时工具
*****************************************************/

using System;
using System.Collections;
using UnityEngine;

namespace TDFramework {

    public partial class MonoBehaviourSimplify {

        /// <summary>
        /// 延时方法
        /// </summary>
        /// <param name="seconds">延时时间</param>
        /// <param name="onFinished">回调</param>
        public void Delay(float seconds, Action onFinished) {
            StartCoroutine(DelayCoroutine(seconds, onFinished));
        }

        private static IEnumerator DelayCoroutine(float seconds, Action onFinished) {
            yield return new WaitForSeconds(seconds);
            onFinished();
        }
    }

    #region 测试
//    public class DelayWithCoroutine : MonoBehaviourSimplify {
//        private void Start() {
//            Delay(5.0f, () => {
//                UnityEditor.EditorApplication.isPlaying = false;
//            });
//        }

//#if UNITY_EDITOR
//        [UnityEditor.MenuItem("TDFramework/11.定时功能", false, 11)]
//        private static void MenuClickd() {
//            UnityEditor.EditorApplication.isPlaying = true;
//            new GameObject("DelayWithCoroutine").AddComponent<DelayWithCoroutine>();
//        }
//#endif
//    }
    #endregion
}
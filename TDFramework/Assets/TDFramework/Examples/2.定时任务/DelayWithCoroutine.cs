/****************************************************
    文件：DelayWithCoroutine.cs
	作者：TravelerTD
    日期：2019/8/7 10:5:36
	功能：定时功能示例
*****************************************************/

using UnityEngine;

namespace TDFramework {
    public class DelayWithCoroutine : MonoBehaviourSimplify {

        private void Start() {
            Delay(5.0f, () => {
                UnityEditor.EditorApplication.isPlaying = false;
            });
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("TDFramework/Example/8.定时功能", false, 9)]
        private static void MenuClickd() {
            UnityEditor.EditorApplication.isPlaying = true;
            new GameObject("DelayWithCoroutine").AddComponent<DelayWithCoroutine>();
        }
#endif
        protected override void OnBeforeDestroy() {
        }
    }
}
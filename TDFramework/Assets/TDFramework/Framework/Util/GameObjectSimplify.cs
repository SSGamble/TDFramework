/****************************************************
    文件：GameObjectSimplify.cs
	作者：TravelerTD
    日期：2019/8/6 11:17:43
	功能：将 SetActive(true/false) 更改为 show，hide
*****************************************************/

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace TDFramework {

    public partial class GameObjectSimplify {

        public static void Show(GameObject gameObj) {
            gameObj.SetActive(true);
        }

        public static void Hide(GameObject gameObj) {
            gameObj.SetActive(false);
        }

        public static void Show(Transform transform) {
            transform.gameObject.SetActive(true);
        }

        public static void Hide(Transform transform) {
            transform.gameObject.SetActive(false);
        }
    }
}
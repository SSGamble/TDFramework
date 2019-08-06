/****************************************************
    文件：MonoBehaviourSimplify.cs
	作者：TravelerTD
    日期：2019/8/6 17:29:36
	功能：MonoBehaviour 扩展基类
*****************************************************/

using UnityEngine;

namespace TDFramework {
    public partial class MonoBehaviourSimplify : MonoBehaviour {

        public void Show() {
            GameObjectSimplify.Show(gameObject);
        }

        public void Hide() {
            GameObjectSimplify.Hide(gameObject);
        }

        public void Identity() {
            TransformSimplify.Identity(transform);
        }
    }
}
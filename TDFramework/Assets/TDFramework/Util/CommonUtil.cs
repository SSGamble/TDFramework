/****************************************************
    文件：CommonUtil.cs
	作者：TravelerTD
    日期：2019/8/6 12:0:43
	功能：通用工具
*****************************************************/

using UnityEngine;

namespace TDFramework {

    public partial class CommonUtil : MonoBehaviour {
        /// <summary>
        /// 复制⽂本到剪切板
        /// </summary>
        /// <param name="text"></param>
        private static void CopyText(string text) {
            GUIUtility.systemCopyBuffer = text;
        }
    }
}
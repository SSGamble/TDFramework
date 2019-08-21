/****************************************************
    文件：FileUtil.cs
	作者：TravelerTD
    日期：2019/8/21 9:40:24
	功能：文件工具类
*****************************************************/

using System;
using System.IO;
using UnityEngine;

namespace TDFramework {

    public class FileUtil : MonoBehaviour {
        /// <summary>
        /// 判断文件是否被占用
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        private static bool FileIsUsed(string path) {
            bool result = false;
            if (!File.Exists(path)) {
                result = false;
            }
            else {
                // 尝试用文件流打开文件，如果出异常说明文件已经被打开了
                FileStream fileStream = null;
                try {
                    fileStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    result = false;
                }
                catch (Exception e) {
                    Debug.LogError(e);
                    result = true;
                }
                finally {
                    if (fileStream != null) {
                        fileStream.Close();
                    }
                }
            }
            return result;
        }
    }
}
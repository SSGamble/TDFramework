/****************************************************
    文件：PercentFunction.cs
	作者：TravelerTD
    日期：2019/8/6 10:58:3
	功能：概率函数，指定百分⽐是否命中概率
*****************************************************/

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace TDFramework {

    public partial class MathUtil {
        /// <summary>
        /// 指定百分⽐是否命中概率
        /// </summary>
        public static bool Percent(int percent) {
            return Random.Range(0, 100) <= percent;
        }

        /// <summary>
        /// 从若⼲个值中随机取出⼀个值
        ///     调用：GetRandomValueFrom(1, 2, 3)
        /// </summary>
        public static T GetRandomValueFrom<T>(params T[] values) {
            return values[Random.Range(0, values.Length)];
        }
    }
}


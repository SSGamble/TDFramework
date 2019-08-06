/****************************************************
    文件：B.cs
	作者：TravelerTD
    日期：2019/8/6 18:13:18
	功能：集成消息机制到 MonoBehaviourSimplify
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDFramework {
    public abstract partial class MonoBehaviourSimplify {
        /// <summary>
        /// 消息
        /// </summary>
        private class MsgRecord {
            public string Name; // 消息名
            public Action<object> OnMsgReceived; // 对应的委托
            // MsgRecord 对象池
            static Stack<MsgRecord> mMsgRecordPool = new Stack<MsgRecord>();
            private MsgRecord() { } // 防止外界 new 对象

            /// <summary>
            /// 申请，获取对象
            /// </summary>
            public static MsgRecord Allocate(string msgName, Action<object> onMsgReceived) {
                MsgRecord retMsgRecord = null;
                retMsgRecord = mMsgRecordPool.Count > 0 ? mMsgRecordPool.Pop() : new MsgRecord();
                retMsgRecord.Name = msgName;
                retMsgRecord.OnMsgReceived = onMsgReceived;
                return retMsgRecord;
            }

            /// <summary>
            /// 回收对象
            /// </summary>
            public void Recycle() {
                Name = null;
                OnMsgReceived = null;
                mMsgRecordPool.Push(this);
            }
        }

        // 已注册的消息
        List<MsgRecord> mMsgRecorder = new List<MsgRecord>();

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="msgName">消息名</param>
        /// <param name="onMsgReceived">回调</param>
        protected void RegisterMsg(string msgName, Action<object> onMsgReceived) {
            MsgDispatcher.Register(msgName, onMsgReceived);
            mMsgRecorder.Add(MsgRecord.Allocate(msgName, onMsgReceived));
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="data"></param>
        protected void SendMsg(string msgName, object data) {
            MsgDispatcher.Send(msgName, data);
        }

        /// <summary>
        /// 注销同⼀消息名下的所有回调
        /// </summary>
        /// <param name="msgName"></param>
        protected void UnRegisterMsg(string msgName) {
            // 在 mMsgRecorder 内查询出所有符合条件的项
            var selectedRecords = mMsgRecorder.FindAll(recorder => recorder.Name == msgName);
            selectedRecords.ForEach(selectRecord => {
                MsgDispatcher.UnRegister(selectRecord.Name, selectRecord.OnMsgReceived);
                mMsgRecorder.Remove(selectRecord);
                selectRecord.Recycle();
            });
            selectedRecords.Clear();
        }

        /// <summary>
        /// 注销指定消息名下的特定回调
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="onMsgReceived"></param>
        protected void UnRegisterMsg(string msgName, Action<object> onMsgReceived) {
            var selectedRecords = mMsgRecorder.FindAll(recorder => recorder.Name == msgName && recorder.OnMsgReceived == onMsgReceived);
            selectedRecords.ForEach(selectRecord => {
                MsgDispatcher.UnRegister(selectRecord.Name, selectRecord.OnMsgReceived);
                mMsgRecorder.Remove(selectRecord);
                selectRecord.Recycle();
            });
            selectedRecords.Clear();
        }

        /// <summary>
        /// 消息的⾃动注销
        /// </summary>
        private void OnDestroy() {
            OnBeforeDestroy();
            // 遍历注销
            foreach (var msgRecord in mMsgRecorder) {
                MsgDispatcher.UnRegister(msgRecord.Name, msgRecord.OnMsgReceived);
                msgRecord.Recycle();
            }
            mMsgRecorder.Clear();
        }

        /// <summary>
        /// 为了提醒⼦类不要覆写了 OnDestroy，而是推荐⽤ OnBeforeDestroy 来做卸载逻辑
        /// </summary>
        protected abstract void OnBeforeDestroy();
    }

    #region 测试
    public class MsgDistapcherTest : MonoBehaviourSimplify {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("TDFramework/消息机制集成到 MonoBehaviourSimplify", false, 14)]
        private static void MenuClicked() {
            UnityEditor.EditorApplication.isPlaying = true;
            new GameObject("MsgReceiverObj").AddComponent<MsgDistapcherTest>();
        }
#endif
        private void Awake() {
            RegisterMsg("Do", DoSomething);
            RegisterMsg("Do", DoSomething);
            RegisterMsg("DO1", _ => { });
            RegisterMsg("DO2", _ => { });
        }

        private IEnumerator Start() {
            SendMsg("Do", "hello");
            yield return new WaitForSeconds(1.0f);
            SendMsg("Do", "hello1");
        }

        void DoSomething(object data) {
            // do something
            Debug.LogFormat("Received Do msg:{0}", data);
        }

        protected override void OnBeforeDestroy() {
        }
    }
    #endregion
}
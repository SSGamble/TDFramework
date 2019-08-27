/****************************************************
    文件：TDTimerDemo.cs
	作者：TravelerTD
    笔记：363
    日期：2019/08/27 09:44:17
	功能：TDTimer 的使用示例
*****************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace TDFramework {

    #region Unity 客户端示例
    public class TDTimerDemo : MonoBehaviour {

        TDTimer tt = new TDTimer();
        int tempID;

        private void Start() {

            // 第一种方式：添加定时任务
            tt.AddTimeTask(TimeTaskFun, 5, TimeUnit.Second, 2); // 时间定时
            tt.AddFrameTask(FrameTaskFun, 100, 1); // 帧数定时

            // 第二种方式：添加时间定时任务，帧任务同理
            tempID = tt.AddTimeTask((int tid) => {
                Debug.Log("定时等待替换......");
            }, 1, TimeUnit.Second, 0);
        }

        private void Update() {
            tt.Update(); // 驱动整个定时任务的检测

            if (Input.GetKeyDown(KeyCode.A)) {
                // 定时任务替换
                bool succ = tt.ReplaceTimeTask(tempID, (int tid) => {
                    Debug.Log("定时任务替换完成......");
                }, 1, TimeUnit.Second, 0);

                if (succ) {
                    Debug.Log("替换成功");
                }
            }

            if (Input.GetKeyDown(KeyCode.D)) {
                // 定时任务删除
                tt.DelTimeTask(tempID);
            }
        }

        void TimeTaskFun(int tid) {
            Debug.Log("TimeTask:" + System.DateTime.UtcNow);
        }

        void FrameTaskFun(int tid) {
            Debug.Log("FrameTask:" + System.DateTime.UtcNow);
        }
    }
    #endregion

    #region 服务器示例
    /// <summary>
    /// 任务数据包
    /// </summary>
    class TaskPack {
        public int tid;
        public Action<int> cb;
        public TaskPack(int tid, Action<int> cb) {
            this.tid = tid;
            this.cb = cb;
        }
    }

    class Program {
        private static readonly string obj = "lock"; // 锁

        static void Main(string[] args) {
            Console.WriteLine("Test Start!");
            //Test1();
            Test2();
        }

        // 第一种用法：运行线程检测并处理任务，不推荐使用
        static void Test1() {
            // 运行线程驱动计时
            TDTimer pt = new TDTimer();
            pt.SetLog((string info) => {
                Console.WriteLine("LogInfo:" + info);
            });

            pt.AddTimeTask((int tid) => {
                Console.WriteLine("Process线程ID:{0}", Thread.CurrentThread.ManagedThreadId.ToString());
            }, 10, TimeUnit.Millisecond, 0);

            while (true) {
                pt.Update();
            }
        }

        // 第二种用法：独立线程检测并处理任务，推荐使用
        static void Test2() {
            Queue<TaskPack> tpQue = new Queue<TaskPack>();
            // 独立线程驱动计时
            TDTimer pt = new TDTimer(5);
            pt.SetLog((string info) => {
                Console.WriteLine("LogInfo:" + info);
            });

            int id = pt.AddTimeTask((int tid) => {
                Console.WriteLine("Process线程ID:{0}", Thread.CurrentThread.ManagedThreadId.ToString());
            }, 3000, TimeUnit.Millisecond, 0);

            // 设置回调处理器
            pt.SetHandle((Action<int> cb, int tid) => {
                if (cb != null) {
                    lock (obj) {
                        tpQue.Enqueue(new TaskPack(tid, cb));
                    }
                }
            });
            while (true) {
                string ipt = Console.ReadLine();
                if (ipt == "a") {
                    pt.DelTimeTask(id);
                }
                if (tpQue.Count > 0) {
                    TaskPack tp = null;
                    lock (obj) {
                        tp = tpQue.Dequeue();
                    }
                    tp.cb(tp.tid);
                }
            }
        }
    }


    #endregion
}
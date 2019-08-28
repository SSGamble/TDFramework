/****************************************************
	文件：GameMapManager.cs
	作者：TravelerTD
	日期：2019/08/18 16:50   	
	功能：场景管理
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameMapManager : Singleton<GameMapManager> {

    /// <summary>
    /// 加载场景完成回调
    /// </summary>
    public Action LoadSceneOverCallBack;
    /// <summary>
    /// 加载场景开始回调
    /// </summary>
    public Action LoadSceneEnterCallBack;
    /// <summary>
    /// 当前场景名
    /// </summary>
    public string CurrentMapName { get; set; }
    /// <summary>
    /// 场景加载是否完成
    /// </summary>
    public bool AlreadyLoadScene { get; set; }
    /// <summary>
    /// 切换场景进度条
    /// </summary>
    public static int LoadingProgress = 0;

    private MonoBehaviour m_Mono;

    /// <summary>
    /// 场景管理初始化
    /// </summary>
    /// <param name="mono"></param>
    public void Init(MonoBehaviour mono) {
        m_Mono = mono;
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="name">场景名</param>
    public void LoadScene(string name) {
        LoadingProgress = 0;
        m_Mono.StartCoroutine(LoadSceneAsync(name));
        UIManager.Instance.PopUpWnd(ConStr.LOADINGPANEL, true, name);
    }

    /// <summary>
    /// 设置场景环境
    /// </summary>
    /// <param name="name"></param>
    void SetSceneSetting(string name) {
        // 设置各种场景环境，可以根据配表来,TODO:
    }

    IEnumerator LoadSceneAsync(string name) {
        // 加载场景开始回调
        if (LoadSceneEnterCallBack != null) {
            LoadSceneEnterCallBack();
        }
        ClearCache();
        AlreadyLoadScene = false;
        // 异步加载场景
        AsyncOperation unLoadScene = SceneManager.LoadSceneAsync(ConStr.EMPTYSCENE, LoadSceneMode.Single); 
        while (unLoadScene != null && !unLoadScene.isDone) {
            yield return new WaitForEndOfFrame();
        }
        // 进度条
        LoadingProgress = 0;
        int targetProgress = 0;
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(name);
        if (asyncScene != null && !asyncScene.isDone) {
            asyncScene.allowSceneActivation = false;
            while (asyncScene.progress < 0.9f) {
                targetProgress = (int)asyncScene.progress * 100;
                yield return new WaitForEndOfFrame();
                // 平滑过渡
                while (LoadingProgress < targetProgress) {
                    ++LoadingProgress;
                    yield return new WaitForEndOfFrame();
                }
            }
            CurrentMapName = name;
            // 设置场景环境
            SetSceneSetting(name);
            targetProgress = 100; // 手动将显示进度设置为 100
            // 自行加载剩余的 10%
            while (LoadingProgress < targetProgress - 2) {
                ++LoadingProgress;
                yield return new WaitForEndOfFrame();
            }
            LoadingProgress = 100;
            asyncScene.allowSceneActivation = true;
            AlreadyLoadScene = true;
            // 加载场景完成回调
            if (LoadSceneOverCallBack != null) {
                LoadSceneEnterCallBack();
            }
        }
    }

    /// <summary>
    /// 跳场景需要清除的东西
    /// </summary>
    private void ClearCache() {
        ObjectManager.Instance.ClearCache();
        ResourceManager.Instance.ClearCache();
    }
}

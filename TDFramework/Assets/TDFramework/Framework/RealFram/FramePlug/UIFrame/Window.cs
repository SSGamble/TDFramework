/****************************************************
	文件：Window.cs
	作者：TravelerTD
	日期：2019/08/18 11:04   	
	功能：UI 框架 - 窗口信息
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window {
    /// <summary>
    /// 引用 GameObject
    /// </summary>
    public GameObject GameObject { get; set; }
    /// <summary>
    /// 引用 Transform
    /// </summary>
    public Transform Transform { get; set; }
    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 所有的 Button
    /// </summary>
    protected List<Button> m_AllButton = new List<Button>();
    /// <summary>
    /// 所有 Toggle
    /// </summary>
    protected List<Toggle> m_AllToggle = new List<Toggle>();

    public virtual bool OnMessage(UIMsgID msgID, params object[] paralist) {
        return true;
    }

    #region 自定义窗口的生命周期函数
    public virtual void Awake(params object[] paralist) { }

    public virtual void OnShow(params object[] paralist) { }

    public virtual void OnDisable() { }

    public virtual void OnUpdate() { }

    public virtual void OnClose() {
        RemoveAllButtonListener();
        RemoveAllToggleListener();
        m_AllButton.Clear();
        m_AllToggle.Clear();
    }
    #endregion

    #region 替换图片
    /// <summary>
    /// 同步替换图片
    /// </summary>
    /// <param name="path"></param>
    /// <param name="image"></param>
    /// <param name="setNativeSize"></param>
    /// <returns>是否替换成功</returns>
    public bool ChangeImageSprite(string path, Image image, bool setNativeSize = false) {
        if (image == null) {
            return false;
        }
        Sprite sp = ResourceManager.Instance.LoadResource<Sprite>(path); // 同步加载图片
        if (sp != null) {
            if (image.sprite != null) { // 置空原有的图片
                image.sprite = null;
            }
            image.sprite = sp; // 替换图片
            if (setNativeSize) {
                image.SetNativeSize();
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 异步替换图片
    /// </summary>
    /// <param name="path"></param>
    /// <param name="image"></param>
    /// <param name="setNativeSize"></param>
    public void ChangImageSpriteAsync(string path, Image image, bool setNativeSize = false) {
        if (image == null) {
            return;
        }
        ResourceManager.Instance.AsyncLoadResource(path, OnLoadSpriteFinish, LoadResPriority.RES_MIDDLE, true, image, setNativeSize);
    }

    /// <summary>
    /// 图片加载完成的回调
    /// </summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    /// <param name="pImage"></param>
    /// <param name="pSetNativeSize"></param>
    /// <param name="param3"></param>
    void OnLoadSpriteFinish(string path, Object obj, object pImage = null, object pSetNativeSize = null, object param3 = null) {
        if (obj != null) {
            Sprite sp = obj as Sprite;
            Image image = pImage as Image;
            bool setNativeSize = (bool)pSetNativeSize;
            if (image.sprite != null) {
                image.sprite = null;
            }
            image.sprite = sp;
            if (setNativeSize) {
                image.SetNativeSize();
            }
        }
    }
    #endregion

    #region 事件的监听和移除
    /// <summary>
    /// 移除所有的 button 事件
    /// </summary>
    public void RemoveAllButtonListener() {
        foreach (Button btn in m_AllButton) {
            btn.onClick.RemoveAllListeners();
        }
    }

    /// <summary>
    /// 移除所有的 toggle 事件
    /// </summary>
    public void RemoveAllToggleListener() {
        foreach (Toggle toggle in m_AllToggle) {
            toggle.onValueChanged.RemoveAllListeners();
        }
    }

    /// <summary>
    /// 添加 button 事件监听
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="action"></param>
    public void AddButtonClickListener(Button btn, UnityEngine.Events.UnityAction action) {
        if (btn != null) {
            if (!m_AllButton.Contains(btn)) {
                m_AllButton.Add(btn);
            }
            btn.onClick.RemoveAllListeners(); // 先清掉身上的事件
            btn.onClick.AddListener(action);
            btn.onClick.AddListener(BtnPlaySound); // 添加按钮的声音事件
        }
    }

    /// <summary>
    /// 添加 toggle 事件监听
    /// </summary>
    /// <param name="toggle"></param>
    /// <param name="action"></param>
    public void AddToggleClickListener(Toggle toggle, UnityEngine.Events.UnityAction<bool> action) {
        if (toggle != null) {
            if (!m_AllToggle.Contains(toggle)) {
                m_AllToggle.Add(toggle);
            }
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(action);
            toggle.onValueChanged.AddListener(TogglePlaySound);
        }
    }
    #endregion

    #region 声音事件
    /// <summary>
    /// 播放 button 声音，即点击 button 播放的声音
    /// </summary>
    void BtnPlaySound() {

    }

    /// <summary>
    /// 播放 toggle 声音，即点击 toggle 播放的声音
    /// </summary>
    /// <param name="isOn"></param>
    void TogglePlaySound(bool isOn) {

    }
    #endregion
}

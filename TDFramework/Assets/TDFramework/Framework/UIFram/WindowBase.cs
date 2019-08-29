/****************************************************
	文件：WindowBase.cs
	作者：TravelerTD
	日期：2019/08/18 11:04   	
	功能：UI 界面基类
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowBase {
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

    public virtual bool OnMessage(UIMsgID msgID, params object[] paralist) {
        return true;
    }

    #region 自定义窗口的生命周期函数
    /// <summary>
    /// 初始化窗口
    /// </summary>
    public virtual void InitWnd(params object[] paralist) { }

    public virtual void OnShow(params object[] paralist) { }

    public virtual void OnDisable() { }

    public virtual void OnUpdate() { }

    /// <summary>
    /// 清理窗口
    /// </summary>
    public virtual void OnClose() { }
    #endregion

    #region 设置 Text 组件的文字
    protected void SetText(Text txt, string context = "") {
        txt.text = context;
    }

    protected void SetText(Text txt, int num = 0) {
        SetText(txt, num.ToString());
    }

    // 获取 Transform 上的 Text 组件上的文字
    protected void SetText(Transform trans, int num = 0) {
        SetText(trans.GetComponent<Text>(), num);
    }

    protected void SetText(Transform trans, string context = "") {
        SetText(trans.GetComponent<Text>(), context);
    }
    #endregion

    #region 激活物体
    protected void SetActive(GameObject go, bool isActive = true) {
        go.SetActive(isActive);
    }
    protected void SetActive(Transform trans, bool state = true) {
        trans.gameObject.SetActive(state);
    }
    protected void SetActive(RectTransform rectTrans, bool state = true) {
        rectTrans.gameObject.SetActive(state);
    }
    protected void SetActive(Image img, bool state = true) {
        img.transform.gameObject.SetActive(state);
    }
    protected void SetActive(Text txt, bool state = true) {
        txt.transform.gameObject.SetActive(state);
    }
    #endregion

    #region 设置图片
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
    void OnLoadSpriteFinish(string path, object obj, object pImage = null, object pSetNativeSize = null, object param3 = null) {
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

    #region 点击事件
    // 为指定物体添加事件监听脚本并设置回调
    protected void OnClickDown(GameObject go, Action<PointerEventData> cb) {
        TDListener listener = GetOrAddComponect<TDListener>(go);
        listener.onClickDown = cb;
    }
    protected void OnClickUp(GameObject go, Action<PointerEventData> cb) {
        TDListener listener = GetOrAddComponect<TDListener>(go);
        listener.onClickUp = cb;
    }
    protected void OnDrag(GameObject go, Action<PointerEventData> cb) {
        TDListener listener = GetOrAddComponect<TDListener>(go);
        listener.onDrag = cb;
    }

    /// <summary>
    /// 带参数点击
    /// </summary>
    /// <param name="go">响应物体</param>
    /// <param name="cb">回调</param>
    /// <param name="args">传递参数</param>
    protected void OnClick(GameObject go, Action<object> cb, object args) {
        TDListener listener = GetOrAddComponect<TDListener>(go);
        listener.onClick = cb;
        listener.args = args;
    }
    #endregion

    /// <summary>
    /// 为物体添加组件，如果已有该组件就获取
    /// where: T 必须要是组件的子类才能被添加
    /// </summary>
    protected T GetOrAddComponect<T>(GameObject go) where T : Component {
        T t = go.GetComponent<T>();
        if (t == null) {
            t = go.AddComponent<T>();
        }
        return t;
    }

}

/****************************************************
	文件：UIManager.cs
	作者：TravelerTD
	日期：2019/08/18 10:08   	
	功能：UI 框架，管理类
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 消息类型
/// </summary>
public enum UIMsgID {
    None = 0,
}

public class UIManager : Singleton<UIManager> {
    /// <summary>
    /// 窗体 Prefab 的路径
    /// </summary>
    private string m_UIPrefabPath = ConStr.PREFAB_UI_PATH;
    /// <summary>
    /// UI 节点
    /// </summary>
    public RectTransform m_UiRoot;
    /// <summary>
    /// 窗口节点
    /// </summary>
    private RectTransform m_WndRoot;
    /// <summary>
    /// UI 摄像机
    /// </summary>
    private Camera m_UICamera;
    /// <summary>
    /// EventSystem 节点
    /// </summary>
    private EventSystem m_EventSystem;
    /// <summary>
    /// 屏幕的宽高比
    /// </summary>
    private float m_CanvasRate = 0;
    /// <summary>
    /// 注册窗口的字典，key：窗口名字，val：窗口类型
    /// </summary>
    private Dictionary<string, System.Type> m_RegisterDic = new Dictionary<string, System.Type>();
    /// <summary>
    /// 所有打开的窗口
    /// </summary>
    private Dictionary<string, WindowBase> m_WindowDic = new Dictionary<string, WindowBase>();
    /// <summary>
    /// 打开的窗口列表，用于遍历关闭
    /// </summary>
    private List<WindowBase> m_WindowList = new List<WindowBase>();

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="uiRoot">UI 父节点</param>
    /// <param name="wndRoot">窗口父节点</param>
    /// <param name="uiCamera">UI 摄像机</param>
    public void Init(RectTransform uiRoot, RectTransform wndRoot, Camera uiCamera, EventSystem eventSystem) {
        m_UiRoot = uiRoot;
        m_WndRoot = wndRoot;
        m_UICamera = uiCamera;
        m_EventSystem = eventSystem;
        m_CanvasRate = Screen.height / (m_UICamera.orthographicSize * 2);
    }

    /// <summary>
    /// 设置所有界面的 UIPrefab 路径
    /// </summary>
    /// <param name="path"></param>
    public void SetUIPrefabPath(string path) {
        m_UIPrefabPath = path;
    }

    /// <summary>
    /// 显示或者隐藏所有 UI
    /// </summary>
    public void ShowOrHideUI(bool show) {
        if (m_UiRoot != null) { // 直接对父节点进行操作
            m_UiRoot.gameObject.SetActive(show);
        }
    }

    /// <summary>
    /// 设置默认选择对象，eg: 以进入菜单界面，就默认选择了某个按钮
    /// </summary>
    /// <param name="obj"></param>
    public void SetNormalSelectObj(GameObject obj) {
        if (m_EventSystem == null) {
            m_EventSystem = EventSystem.current;
        }
        m_EventSystem.firstSelectedGameObject = obj;
    }

    /// <summary>
    /// 窗口的更新
    /// </summary>
    public void OnUpdate() {
        for (int i = 0; i < m_WindowList.Count; i++) {
            if (m_WindowList[i] != null) {
                m_WindowList[i].OnUpdate();
            }
        }
    }

    /// <summary>
    /// 窗口注册方法
    /// </summary>
    /// <typeparam name="T">窗口泛型类</typeparam>
    /// <param name="name">窗口名</param>
    public void Register<T>(string name) where T : WindowBase {
        m_RegisterDic[name] = typeof(T);
    }

    /// <summary>
    /// 发送消息给窗口
    /// </summary>
    /// <param name="name">窗口名</param>
    /// <param name="msgID">消息 ID</param>
    /// <param name="paralist">参数数组</param>
    /// <returns></returns>
    public bool SendMessageToWnd(string name, UIMsgID msgID = 0, params object[] paralist) {
        WindowBase wnd = FindWndByName<WindowBase>(name);
        if (wnd != null) {
            return wnd.OnMessage(msgID, paralist);
        }
        return false;
    }

    /// <summary>
    /// 根据窗口名查找窗口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T FindWndByName<T>(string name) where T : WindowBase {
        WindowBase wnd = null;
        if (m_WindowDic.TryGetValue(name, out wnd)) {
            return (T)wnd;
        }
        return null;
    }

    /// <summary>
    /// 打开窗口
    /// </summary>
    /// <param name="wndName">窗口名字</param>
    /// <param name="bTop">是否在最上面</param>
    /// <param name="paralist">可传递的参数</param>
    /// <returns></returns>
    public WindowBase PopUpWnd(string wndName, bool bTop = true, params object[] paralist) {
        WindowBase wnd = FindWndByName<WindowBase>(wndName);
        if (wnd == null) {
            System.Type tp = null;
            if (m_RegisterDic.TryGetValue(wndName, out tp)) {
                wnd = System.Activator.CreateInstance(tp) as WindowBase;
                if (wnd == null) {
                    Debug.LogError("PopUpWnd：未反射出窗口");
                }
            }
            else {
                Debug.LogError("找不到窗口对应的脚本，窗口名是：" + wndName + "，可能没有注册");
                return null;
            }
            // 加载窗口，窗口的加载应该是同步的
            GameObject wndObj = ObjectManager.Instance.InstantiateObject(m_UIPrefabPath + wndName, false, false);
            if (wndObj == null) {
                Debug.Log("创建窗口 Prefab 失败：" + wndName);
                return null;
            }
            if (!m_WindowDic.ContainsKey(wndName)) {
                m_WindowList.Add(wnd);
                m_WindowDic.Add(wndName, wnd);
            }
            wnd.GameObject = wndObj;
            wnd.Transform = wndObj.transform;
            wnd.Name = wndName;
            wnd.InitWnd(paralist);
            wndObj.transform.SetParent(m_WndRoot, false);
            if (bTop) {
                wndObj.transform.SetAsLastSibling();
            }
            wnd.OnShow(paralist);
        }
        else {
            ShowWnd(wndName, bTop, paralist);
        }
        return wnd;
    }

    #region 关闭窗口
    /// <summary>
    /// 根据窗口名关闭窗口
    /// </summary>
    /// <param name="name"></param>
    /// <param name="destory"></param>
    public void CloseWnd(string name, bool destory = false) {
        WindowBase wnd = FindWndByName<WindowBase>(name);
        CloseWnd(wnd, destory);
    }

    /// <summary>
    /// 根据窗口对象关闭窗口
    /// </summary>
    /// <param name="window"></param>
    /// <param name="destory"></param>
    public void CloseWnd(WindowBase window, bool destory = false) {
        if (window != null) {
            window.OnDisable();
            window.OnClose();
            if (m_WindowDic.ContainsKey(window.Name)) {
                m_WindowDic.Remove(window.Name);
                m_WindowList.Remove(window);
            }
            if (destory) {
                ObjectManager.Instance.ReleaseObject(window.GameObject, 0, true);
            }
            else {
                ObjectManager.Instance.ReleaseObject(window.GameObject, recycleParent: false);
            }
            window.GameObject = null;
            window = null;
        }
    }

    /// <summary>
    /// 关闭所有窗口
    /// </summary>
    public void CloseAllWnd() {
        for (int i = m_WindowList.Count - 1; i >= 0; i--) {
            CloseWnd(m_WindowList[i]);
        }
    }
    #endregion

    /// <summary>
    /// 切换到唯一窗口
    /// </summary>
    public void SwitchStateByName(string name, bool bTop = true, params object[] paralist) {
        CloseAllWnd();
        PopUpWnd(name, bTop, paralist);
    }

    /// <summary>
    /// 根据名字隐藏窗口
    /// </summary>
    /// <param name="name"></param>
    public void HideWnd(string name) {
        WindowBase wnd = FindWndByName<WindowBase>(name);
        HideWnd(wnd);
    }

    /// <summary>
    /// 根据窗口对象隐藏窗口
    /// </summary>
    /// <param name="wnd"></param>
    public void HideWnd(WindowBase wnd) {
        if (wnd != null) {
            wnd.GameObject.SetActive(false);
            wnd.OnDisable();
        }
        else {
            Debug.LogError("wnd == null");
        }
    }

    /// <summary>
    /// 根据窗口名字显示窗口
    /// </summary>
    /// <param name="name"></param>
    /// <param name="paralist"></param>
    public void ShowWnd(string name, bool bTop = true, params object[] paralist) {
        WindowBase wnd = FindWndByName<WindowBase>(name);
        ShowWnd(wnd, bTop, paralist);
    }

    /// <summary>
    /// 根据窗口对象显示窗口
    /// </summary>
    /// <param name="wnd"></param>
    /// <param name="paralist"></param>
    public void ShowWnd(WindowBase wnd, bool bTop = true, params object[] paralist) {
        if (wnd != null) {
            if (wnd.GameObject != null && !wnd.GameObject.activeSelf) {
                wnd.GameObject.SetActive(true);
            }
            if (bTop) {
                wnd.Transform.SetAsLastSibling();
            }
            wnd.OnShow(paralist);
        }
    }
}

/****************************************************
	文件：UIDemoGameStart.cs
	作者：TravelerTD
	日期：2019/08/18 16:31   	
	功能：游戏入口
*****************************************************/
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDemoGameStart : MonoBehaviour {
    private GameObject m_obj;

    private void Awake() {
        GameObject.DontDestroyOnLoad(gameObject); // 跳场景不卸载
        AssetBundleManager.Instance.LoadAssetBundleConfig(); // 加载 AssetBundle 配置表
        ResourceManager.Instance.Init(this); // 异步加载，初始化
        ObjectManager.Instance.Init(transform.Find("RecyclePoolTrs"), transform.Find("SceneTrs"));
    }

    private void Start() {
        // 加载配置表
        LoadConfiger();
        // 初始化 UI
        UIManager.Instance.Init(transform.Find("UIRoot") as RectTransform, transform.Find("UIRoot/WndRoot") as RectTransform, transform.Find("UIRoot/UICamera").GetComponent<Camera>(), transform.Find("UIRoot/EventSystem").GetComponent<EventSystem>());
        // 注册 UI 窗口
        RegisterUI();
        // 初始化 场景管理
        GameMapManager.Instance.Init(this);
        Test();
        // 加载场景
        GameMapManager.Instance.LoadScene(ConStr.MENUSCENE);
    }

    void Update() {
        UIManager.Instance.OnUpdate();
    }


    /// <summary>
    /// 预加载
    /// </summary>
    private void Test() {
        ObjectManager.Instance.PreloadGameObject(ConStr.ATTACK, 5);
    }

    /// <summary>
    /// 加载/卸载 Attack Prefab
    /// </summary>
    private void Test2() {
        GameObject obj = ObjectManager.Instance.InstantiateObject(ConStr.ATTACK, true);
        ObjectManager.Instance.ReleaseObject(obj, 0, true);
        obj = null;
    }

    /// <summary>
    /// 预加载资源 - 音乐
    /// </summary>
    private void Test3() {
        ResourceManager.Instance.PreloadRes(ConStr.MENUSOUND);
    }

    /// <summary>
    /// 同步加载资源 - 音乐
    /// </summary>
    private void Test4() {
        AudioClip clip= ResourceManager.Instance.LoadResource<AudioClip>(ConStr.MENUSOUND);
        ResourceManager.Instance.ReleaseResouce(clip);
    }

    /// <summary>
    /// 注册 UI 窗口
    /// </summary>
    void RegisterUI() {
        UIManager.Instance.Register<MenuUi>(ConStr.MENUPANEL);
        UIManager.Instance.Register<LoadingUi>(ConStr.LOADINGPANEL);
    }

    /// <summary>
    /// 加载配置表
    /// </summary>
    void LoadConfiger() {
        ConfigerManager.Instance.LoadData<MonsterData>(CFG.TABLE_MONSTER);
        ConfigerManager.Instance.LoadData<BuffData>(CFG.TABLE_BUFF);
    }
    

    private void OnApplicationQuit() {
#if UNITY_EDITOR
        ResourceManager.Instance.ClearCache();
        Resources.UnloadUnusedAssets();
        Debug.Log("清空编辑器缓存");
#endif
    }
}

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
        //AssetBundleManager.Instance.LoadAssetBundleConfig();
        //ResourceManager.Instance.Init(this);
        ObjectManager.Instance.Init(transform.Find("RecyclePoolTrs"), transform.Find("SceneTrs")); // 归置对象池对象位置
    }

    private void Start() {
        // 加载配置表
        LoadConfiger();
        // 初始化 UI
        UIManager.Instance.Init(transform.Find("UIRoot") as RectTransform, transform.Find("UIRoot/WndRoot") as RectTransform, transform.Find("UIRoot/UICamera").GetComponent<Camera>(), transform.Find("UIRoot/EventSystem").GetComponent<EventSystem>());
        // 注册 UI 窗口
        RegisterUI();
        UIManager.Instance.PopUpWnd(ConStr.MAIN_PANEL);
    }

    void Update() {
        UIManager.Instance.OnUpdate();
    }

    /// <summary>
    /// 注册 UI 窗口
    /// </summary>
    void RegisterUI() {
        UIManager.Instance.Register<TaskWnd>(ConStr.TASK_PANEL);
        UIManager.Instance.Register<MainWnd>(ConStr.MAIN_PANEL);
        UIManager.Instance.Register<SkillWnd>(ConStr.SKILL_PANEL);
    }

    /// <summary>
    /// 加载配置表
    /// </summary>
    void LoadConfiger() {
        //ConfigerManager.Instance.LoadData<MonsterData>(CFG.TABLE_MONSTER);
        //ConfigerManager.Instance.LoadData<BuffData>(CFG.TABLE_BUFF);
        Debug.Log("加载配置表完成...");
    }

    private void OnApplicationQuit() {
#if UNITY_EDITOR
        ResourceManager.Instance.ClearCache();
        Resources.UnloadUnusedAssets();
        Debug.Log("清空编辑器缓存");
#endif
    }
}

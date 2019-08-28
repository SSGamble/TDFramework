/****************************************************
	文件：MenuUi.cs
	作者：TravelerTD
	日期：2019/08/18 16:27   	
	功能：MenuPanel 的窗口类
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUi : Window {
    private MenuPanel m_MainPanel;
    private AudioClip m_Clip;

    public override void Awake(params object[] paralist) {
        m_MainPanel = GameObject.GetComponent<MenuPanel>();
        AddButtonClickListener(m_MainPanel.m_StartButton, OnClickStart);
        AddButtonClickListener(m_MainPanel.m_LoadButton, OnClickLoad);
        AddButtonClickListener(m_MainPanel.m_ExitButton, OnClickExit);

        LoadPrefabTest();

        // 加载优先级测试，3 1 2
        ResourceManager.Instance.AsyncLoadResource("Assets/GameData/UGUI/Test1.png", OnLoadSpriteTest1, LoadResPriority.RES_MIDDLE, true);
        ResourceManager.Instance.AsyncLoadResource("Assets/GameData/UGUI/test2.png", OnLoadSpriteTest2, LoadResPriority.RES_SLOW, true);
        ResourceManager.Instance.AsyncLoadResource("Assets/GameData/UGUI/test3.png", OnLoadSpriteTest3, LoadResPriority.RES_HIGHT, true);

        LoadMonsterData();
    }


    /// <summary>
    /// 加载音频
    /// </summary>
    private void LoadSoundTest() {
        m_Clip = ResourceManager.Instance.LoadResource<AudioClip>(ConStr.MENUSOUND);
        m_MainPanel.m_Audio.clip = m_Clip;
        m_MainPanel.m_Audio.Play();
    }

    /// <summary>
    /// 加载 Prefab
    /// </summary>
    private void LoadPrefabTest() {
        GameObject go = ObjectManager.Instance.InstantiateObject("Assets/GameData/Prefabs/Attack.prefab", true); // 从预加载中拿
        ObjectManager.Instance.ReleaseObject(go, 0, true);
        ObjectManager.Instance.ClearCache();
        ResourceManager.Instance.ClearCache();
    }

    /// <summary>
    /// 加载配置数据测试
    /// </summary>
    void LoadMonsterData() {
        MonsterData monsterData = ConfigerManager.Instance.FindData<MonsterData>(CFG.TABLE_MONSTER);
        Debug.Log("加载配置数据: " + monsterData);
        for (int i = 0; i < monsterData.AllMonster.Count; i++) {
            Debug.Log(string.Format("ID:{0} 名字：{1}  外观：{2}  高度：{3}  稀有度：{4}", monsterData.AllMonster[i].Id, monsterData.AllMonster[i].Name, monsterData.AllMonster[i].OutLook, monsterData.AllMonster[i].Height, monsterData.AllMonster[i].Rare));
        }
    }

    #region 加载图片
    void OnLoadSpriteTest1(string path, Object obj, object param1 = null, object param2 = null, object param3 = null) {
        if (obj != null) {
            Sprite sp = obj as Sprite;
            m_MainPanel.m_Test1.sprite = sp;
            Debug.Log("图片1加载出来了");
        }
    }

    void OnLoadSpriteTest3(string path, Object obj, object param1 = null, object param2 = null, object param3 = null) {
        if (obj != null) {
            Sprite sp = obj as Sprite;
            m_MainPanel.m_Test3.sprite = sp;
            Debug.Log("图片3加载出来了");
        }
    }

    void OnLoadSpriteTest2(string path, Object obj, object param1 = null, object param2 = null, object param3 = null) {
        if (obj != null) {
            Sprite sp = obj as Sprite;
            m_MainPanel.m_Test2.sprite = sp;
            Debug.Log("图片2加载出来了");
        }
    }
    #endregion

    public override void OnUpdate() {
        if (Input.GetKeyDown(KeyCode.A)) {
            ResourceManager.Instance.ReleaseResouce(m_MainPanel.m_Test1.sprite, true);
            m_MainPanel.m_Test1.sprite = null;
        }
        if (Input.GetKeyDown(KeyCode.B)) { // 卸载音频
            ResourceManager.Instance.ReleaseResouce(m_Clip, true);
            m_MainPanel.m_Audio.clip = null;
            m_Clip = null;
        }
        if(Input.GetKeyDown(KeyCode.R)) { // 释放图片
            ResourceManager.Instance.ReleaseResouce(m_MainPanel.m_Test1.sprite, true);
            m_MainPanel.m_Test1.sprite = null; // 编辑器下需要置空
        }
    }

    void OnClickStart() {
        Debug.Log("点击了开始游戏！");
    }

    void OnClickLoad() {
        Debug.Log("点击了加载游戏！");
    }

    void OnClickExit() {
        Debug.Log("点击了退出游戏！");
    }
}

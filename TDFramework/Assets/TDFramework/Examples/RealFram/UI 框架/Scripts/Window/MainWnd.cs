/****************************************************
    文件：MainWnd.cs
	作者：TravelerTD
    日期：2019/08/29 17:17:16
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.EventSystems;

public class MainWnd : WindowBase {
    private MainPanel mainPanel;

    public override void InitWnd(params object[] paralist) {
        mainPanel = GameObject.GetComponent<MainPanel>();
        OnClickDown(mainPanel.btnTask.gameObject, (PointerEventData evt) => {
            UIManager.Instance.PopUpWnd(ConStr.TASK_PANEL);
        });
        OnClickDown(mainPanel.btnSkill.gameObject, (PointerEventData evt) => {
            UIManager.Instance.PopUpWnd(ConStr.SKILL_PANEL);
        });
    }

    public override void OnShow(params object[] paralist) {


    }

    public override void OnUpdate() {

    }

}
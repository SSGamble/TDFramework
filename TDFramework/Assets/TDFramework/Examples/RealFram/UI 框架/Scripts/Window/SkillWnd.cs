/****************************************************
    文件：SkillWnd.cs
	作者：TravelerTD
    日期：2019/08/29 17:22:09
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.EventSystems;

public class SkillWnd : WindowBase {
    private SkillPanel skillPanel;

    public override void InitWnd(params object[] paralist) {
        skillPanel = GameObject.GetComponent<SkillPanel>();
        OnClickDown(skillPanel.btnClose.gameObject, (PointerEventData evt) => {
            UIManager.Instance.HideWnd(ConStr.SKILL_PANEL);
        });
    }

    public override void OnShow(params object[] paralist) {
        SetText(skillPanel.text, "技能窗口-");
    }

    public override void OnClose() {
        
    }
}
/****************************************************
	文件：LoadingUi.cs
	作者：TravelerTD
	日期：2019/08/18 17:14   	
	功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TaskWnd : WindowBase {
    private TaskPanel taskPanel;

    public override void InitWnd(params object[] paralist) {
        taskPanel = GameObject.GetComponent<TaskPanel>();
        OnClickDown(taskPanel.btnClose.gameObject, (PointerEventData evt) => {
            UIManager.Instance.HideWnd(ConStr.TASK_PANEL);
        });
    }

    public override void OnShow(params object[] paralist) {
        SetText(taskPanel.text, "任务窗口-");
    }

    public override void OnClose() {
        
    }
}

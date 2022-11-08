using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageWall : MonoBehaviour
{
    [SerializeField]
    private StageType wallType;
    MeshRenderer[] renderers;
    GameObject trigger;

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        if (StageManager.Instance != null)
        {
            UIManager.EventHandler.RemoveEvent(UI_EventHandler.UIEventType.ChangeClear);
            UIManager.EventHandler.RemoveEvent(UI_EventHandler.UIEventType.AccessStage);
        }
    }

    private void Init()
    {
        renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        trigger = gameObject.GetComponentInChildren<StageTrigger>().gameObject;
        trigger.SetActive(false);

        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeClear, OnCheckClear);
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.AccessStage, OnAccessNextStage);
    }

    public void OnCheckClear(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        if ((bool)param != true)
            return;
        if (StageManager.Instance.CurrentStage != wallType)
            return;

        // 사용자에게 시각적으로 통행 가능하다는 알림을 주기 위해 Mesh Renderer를 비활성화함
        // 사용자가 해당 오브젝트를 지나갈 수 있도록 Collider를 비활성화함
        foreach (var renderer in renderers)
            renderer.enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;

        // 다음 스테이지 실행 여부를 판단하기 위해 트리거 오브젝트를 활성화함
        trigger.SetActive(true);
    }

    public void OnAccessNextStage(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        if ((StageType)param != wallType)
            return;

        foreach (var renderer in renderers)
            renderer.enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;

        // 트리거 오브젝트가 이벤트를 호출할 경우 다음 스테이지를 실행
        StageManager.Instance.ActiveStage(wallType + 1);
    }
}

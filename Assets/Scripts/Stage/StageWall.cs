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

        // ����ڿ��� �ð������� ���� �����ϴٴ� �˸��� �ֱ� ���� Mesh Renderer�� ��Ȱ��ȭ��
        // ����ڰ� �ش� ������Ʈ�� ������ �� �ֵ��� Collider�� ��Ȱ��ȭ��
        foreach (var renderer in renderers)
            renderer.enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;

        // ���� �������� ���� ���θ� �Ǵ��ϱ� ���� Ʈ���� ������Ʈ�� Ȱ��ȭ��
        trigger.SetActive(true);
    }

    public void OnAccessNextStage(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        if (StageManager.Instance.CurrentStage != wallType)
            return;

        foreach (var renderer in renderers)
            renderer.enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;

        // Ʈ���� ������Ʈ�� �̺�Ʈ�� ȣ���� ��� ���� ���������� ����
        // Event�� �ص� �� �� ������, Manager�� ����ϴٺ��� Singleton�� �������� �ڵ尡 ��������� �� ����
        StageManager.Instance.ActiveStage(wallType + 1);
    }
}

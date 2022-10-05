using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTest : MonoBehaviour
{
    [SerializeField]
    private StageType wallType;
    MeshRenderer[] renderers;

    private void Start()
    {
        Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Sondol"))
            return;

        // �������� ������Ʈ Trigger�� ������ ��� ���� ���������� ����
        // Event�� �ص� �� �� ������, Manager�� ����ϴٺ��� Singleton�� �������� �ڵ尡 ��������� �� ����
        StageManager.Instance.ActiveStage(wallType + 1);

        Utils.Destroy(gameObject, Time.deltaTime);
    }

    private void Init()
    {
        renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeClear, OnCheckClear);
    }

    public void OnCheckClear(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        if ((bool)param != true)
            return;
        if (StageManager.Instance.CurrentStage != wallType)
            return;

        // ����ڿ��� �ð������� ���� �����ϴٴ� �˸��� �ֱ� ���� Mesh Renderer�� ��Ȱ��ȭ��
        foreach (var renderer in renderers)
            renderer.enabled = false;

        // ����ڰ� �ش� ������Ʈ�� ������ �� �ֵ��� IsTriiger�� True�� ������
        gameObject.GetComponent<Collider>().isTrigger = true;
    }
}

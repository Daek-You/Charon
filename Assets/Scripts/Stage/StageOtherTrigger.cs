using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOtherTrigger : MonoBehaviour
{
    [SerializeField]
    private StageType wallType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Sondol"))
            return;

        // Ʈ���� ������Ʈ�� �̺�Ʈ�� ȣ���� ��� ���� ���������� ����
        StageManager.Instance.ActiveStage(wallType + 1);
        gameObject.SetActive(false);
    }
}

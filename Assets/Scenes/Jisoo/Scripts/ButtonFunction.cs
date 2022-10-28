using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunction : MonoBehaviour
{
    public void OnUpgradeMaxHP()
    {
        StatManager.Instance.UpgradeStatus(StatType.MAX_HP);
        Debug.Log("�ִ�ü�� ����");
    }

    public void OnUpgradeArmor()
    {
        StatManager.Instance.UpgradeStatus(StatType.ARMOR);
        Debug.Log("���� ����");
    }

    public void OnUpgradeMoveSpeed()
    {
        StatManager.Instance.UpgradeStatus(StatType.MOVE_SPEED);
        Debug.Log("�̵��ӵ� ����");
    }

    public void OnUpgradeDashCount()
    {
        StatManager.Instance.UpgradeStatus(StatType.DASH_COUNT);
        Debug.Log("�뽬 Ƚ�� ����");
    }

    public void OnUpgradePaddle()
    {
        //StatManager.Instance.UpgradeStatus(StatType.PADDLE_DAMAGE);
    }

    public void OnUpgradeSickle()
    {
        //StatManager.Instance.UpgradeStatus(StatType.SICKLE_DAMAGE);
    }

    public void OnUpgradeGourd()
    {
        //StatManager.Instance.UpgradeStatus(StatType.GOURD_DAMAGE);
    }
}

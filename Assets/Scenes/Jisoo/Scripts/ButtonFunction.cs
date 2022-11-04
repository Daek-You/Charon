using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunction : MonoBehaviour
{
    public Texture[] textures = new Texture[3];
    public Sprite[] sprites = new Sprite[3];
    public RawImage img;
    public void OnUpgradeMaxHP()
    {
        StatManager.Instance.UpgradeStatus(StatType.MAX_HP);
        //Debug.Log("�ִ�ü�� ����");
    }

    public void OnUpgradeArmor()
    {
        StatManager.Instance.UpgradeStatus(StatType.ARMOR);
        //Debug.Log("���� ����");
    }

    public void OnUpgradeMoveSpeed()
    {
        StatManager.Instance.UpgradeStatus(StatType.MOVE_SPEED);
        //Debug.Log("�̵��ӵ� ����");
    }

    public void OnUpgradeDashCount()
    {
        StatManager.Instance.UpgradeStatus(StatType.DASH_COUNT);
        //Debug.Log("�뽬 Ƚ�� ����");
    }

    public void OnUpgradePaddle()
    {
        StatManager.Instance.UpgradeStatus(StatType.PADDLE_DAMAGE);
    }

    public void OnUpgradeSickle()
    {
        StatManager.Instance.UpgradeStatus(StatType.SICKLE_DAMAGE);
    }

    public void OnUpgradeGourd()
    {
        StatManager.Instance.UpgradeStatus(StatType.GOURD_DAMAGE);
    }
    public void OnShowWeaponImgPaddle()
    {
        img.texture = textures[0];
    }
    public void OnShowWeaponImgSickle()
    {
        img.texture = textures[1];
    }
    public void OnShowWeaponImgGourd()
    {
        img.texture = textures[2];
    }
    public void OnCloseWindow()
    {

    }
}

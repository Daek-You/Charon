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
        //Debug.Log("최대체력 증가");
    }

    public void OnUpgradeArmor()
    {
        StatManager.Instance.UpgradeStatus(StatType.ARMOR);
        //Debug.Log("방어력 증가");
    }

    public void OnUpgradeMoveSpeed()
    {
        StatManager.Instance.UpgradeStatus(StatType.MOVE_SPEED);
        //Debug.Log("이동속도 증가");
    }

    public void OnUpgradeDashCount()
    {
        StatManager.Instance.UpgradeStatus(StatType.DASH_COUNT);
        //Debug.Log("대쉬 횟수 증가");
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

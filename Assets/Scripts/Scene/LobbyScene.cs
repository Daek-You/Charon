using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        UIManager.Instance.ShowSceneUI<UI_InGame>();
        UIManager.Instance.ShowSceneUI<UI_AchievementCompletionNotifier>();
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeScene, OnChangeScene);
        StageManager.Instance.CurrentStage = StageType.Lobby;

        GameData saveData = DataManager.Instance.SaveData;
        GameObject weapon = null;

        Player.Instance.transform.position = saveData.CurrentPosition;

        if (!saveData.IsSaved)
        {
            weapon = Utils.Instantiate($"Weapons/CharonPaddle");
            Player.Instance.weaponManager.RegisterWeapon(weapon);
            Player.Instance.weaponManager.SetWeapon(weapon);
            DataManager.Instance.SaveGameData(DataManager.Instance.DataIndex, false);
            return;
        }

        weapon = Utils.Instantiate($"Weapons/{saveData.WeaponName}");
        Player.Instance.weaponManager.RegisterWeapon(weapon);
        Player.Instance.weaponManager.SetWeapon(weapon);

        // 체력도 반영되어야 함
        Player.Instance.weaponManager.Weapon.CurrentSkillGauge = saveData.CurrentST;
    }

    public override void Clear()
    {
        UIManager.Instance.Clear();
    }
}

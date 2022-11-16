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

        CreateEventSystem();

        GameData saveData = DataManager.Instance.SaveData;
        GameObject weapon = null;

        Player.Instance.transform.position = saveData.CurrentPosition;

        // 새로하기로 진입하였을 경우
        if (!saveData.IsSaved)
        {
            weapon = Utils.Instantiate($"Weapons/CharonPaddle");
            Player.Instance.weaponManager.RegisterWeapon(weapon);
            Player.Instance.weaponManager.SetWeapon(weapon);
            StatManager.Instance.SetReinforceLevel();
            StatManager.Instance.Gold = 8100;
            DataManager.Instance.SaveGameData(DataManager.Instance.DataIndex, false);
            // UIManager.Instance.ShowPopupUI<UI_StageName>();
            return;
        }

        // 엔딩 이후 혹은 게임 오버 후 진입하였을 경우
        if (DataManager.Instance.IsDirOrClear)
        {
            Player.Instance.transform.position = new Vector3(0, 0, 0);
            Player.Instance.LoadCurrentHp(Player.Instance.MaxHP);
            Player.Instance.weaponManager.Weapon.CurrentSkillGauge = 0;
            DataManager.Instance.IsDirOrClear = false;
            return;
        }

        Player.Instance.weaponManager.LoadWeaponDictionary();
        weapon = Utils.Instantiate($"Weapons/{saveData.WeaponName}");
        Player.Instance.weaponManager.RegisterWeapon(weapon);
        Player.Instance.weaponManager.SetWeapon(weapon);

        StatManager.Instance.SetReinforceLevel(saveData);
        StatManager.Instance.Gold = saveData.Gold;
        Player.Instance.LoadCurrentHp(saveData.CurrentHP);
        Player.Instance.weaponManager.Weapon.CurrentSkillGauge = saveData.CurrentST;

        // UIManager.Instance.ShowPopupUI<UI_StageName>();
    }

    public override void Clear()
    {
        UIManager.Instance.Clear();
    }
}

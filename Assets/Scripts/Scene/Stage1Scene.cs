using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Scene : BaseScene
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

        CreateEventSystem();

        if (StageManager.Instance.CurrentStage == StageType.Lobby)
        {
            Player.Instance.transform.position = new Vector3(0, 0, -10);

            StageManager.Instance.CurrentStage = StageType.Stage11;
            StageManager.Instance.SetStage();

            //UIManager.Instance.ShowPopupUI<UI_StageName>();
            return;
        }

        GameData saveData = DataManager.Instance.SaveData;

        Player.Instance.transform.position = saveData.CurrentPosition;
        Player.Instance.weaponManager.LoadWeaponDictionary();
        GameObject weapon = Utils.Instantiate($"Weapons/{saveData.WeaponName}");
        Player.Instance.weaponManager.RegisterWeapon(weapon);
        Player.Instance.weaponManager.SetWeapon(weapon);

        StatManager.Instance.SetReinforceLevel(saveData);
        StatManager.Instance.Gold = saveData.Gold;
        Player.Instance.LoadCurrentHp(saveData.CurrentHP);
        Player.Instance.weaponManager.Weapon.CurrentSkillGauge = saveData.CurrentST;

        StageManager.Instance.SetStage();
    }

    public override void Clear()
    {
        UIManager.Instance.Clear();
    }
}

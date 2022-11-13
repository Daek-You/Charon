using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SaveSlot : UI_Base
{
    public UI_ManageData.DataState SlotType { get; set; }
    public int SlotIndex { get; set; }
    bool isExist = false;

    enum Images
    {
        ImgSlot
    }

    enum Texts
    {
        TxtIndex,
        TxtReinforecInfo,
        TxtStageInfo
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        GameObject image = GetImage((int)Images.ImgSlot).gameObject;

        // switch 문은 type이 늘어날수록 비효율적이므로 모듈화할 필요가 있음
        // 여유가 된다면 모듈화 구조에 대해서 생각해볼 것
        switch (SlotType)
        {
            case UI_ManageData.DataState.NewGame:
                BindEvent(image, OnStartNewGame, UIEvent.Click);
            break;
            case UI_ManageData.DataState.Continue:
                BindEvent(image, OnContinueGame, UIEvent.Click);
            break;
            case UI_ManageData.DataState.SaveGame:
                BindEvent(image, OnSaveCurrentGame, UIEvent.Click);
            break;
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        GameData slotData = DataManager.Instance.LoadGameData(SlotIndex);
        isExist = slotData.IsSaved ? true : false;

        if (isExist)
        {
            GetText((int)Texts.TxtIndex).text = $"{SlotIndex + 1}번 데이터";
            GetText((int)Texts.TxtReinforecInfo).text = MakeReinforceText(slotData);
            GetText((int)Texts.TxtStageInfo).text = MakeStageText();
        }
        else
        {
            GetText((int)Texts.TxtIndex).text = "데이터 없음";
            GetText((int)Texts.TxtReinforecInfo).text = "";
            GetText((int)Texts.TxtStageInfo).text = "";
        }
    }


    public string MakeReinforceText(GameData slotData)
    {
        // 게임 중 같은 씬에서 저장할 경우 그 때마다 호출 (빈도 높을 수 있음)
        // 무기를 Poolable로 만든다면 2번째 호출부터는 부하가 줄어들 것
        GameObject go = Utils.Instantiate($"Weapons/{DataManager.Instance.SaveData.WeaponName}");
        string weaponName = go.GetComponent<CharonPaddle>().Name;
        Utils.Destroy(go);

        string text = $"{weaponName} +{DataManager.Instance.SaveData.CurrentWeaponReinforecLevel}\n"
            + $"체력 +{slotData.CurrentHPReinforceLevel}\n"
            + $"방어력 +{slotData.CurrentArmorReinforceLevel}\n"
            + $"이동속도 +{slotData.CurrentMoveSpeedReinforceLevel}\n"
            + $"대시 +{slotData.CurrentDashCountReinforceLevel}";
        return text;
    }

    public string MakeStageText()
    {
        string text = "";
        StageType type = DataManager.Instance.SaveData.CurrentStage;

        if (type == StageType.Lobby)
        {
            text = "로비";
        }
        else if (type != StageType.Unknown && type != StageType.Ending && type != StageType.Title && type != StageType.Loading && type != StageType.Opening)
        {
            text = DataManager.Instance.SaveData.CurrentStage.ToString();
            text = text.Substring(text.Length - 2);
            text = $"스테이지\n{text[0]}-{text[1]}";
        }

        return text;
    }

    public void OnStartNewGame(PointerEventData data)
    {
        DataManager.Instance.DataIndex = SlotIndex;

        if (isExist)
            UIManager.Instance.ShowPopupUI<UI_CheckMessage>();
        else
        {
            DataManager.Instance.StartGameData();
            FadeInOutController.Instance.FadeOutAndLoadScene("OpeningScene", StageType.Opening);
            //LoadingScene.LoadScene("OpeningScene");
            //UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "OpeningScene");
            // GameObject.Find("Sondol").transform.position = DataManager.Instance.SaveData.CurrentPosition;
        }
    }

    public void OnContinueGame(PointerEventData data)
    {
        DataManager.Instance.DataIndex = SlotIndex;

        // 데이터가 있을 때만 해당 게임 불러오기 (없을 경우 무반응)
        if (isExist)
        {
            DataManager.Instance.LoadGameData(SlotIndex);
            GameData saveData = DataManager.Instance.SaveData;

            StageManager.Instance.CurrentStage = saveData.CurrentStage;
            StageManager.Instance.IsClearedByLoad = saveData.IsCleared;

            if (saveData.CurrentStage == StageType.Lobby)
                FadeInOutController.Instance.FadeOutAndLoadScene("LobbyScene", StageType.Lobby);
            //UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "LobbyScene");
            else
                FadeInOutController.Instance.FadeOutAndLoadScene("Stage1Scene", StageType.Stage11);
            //UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "Stage1Scene");
        }
    }

    public void OnSaveCurrentGame(PointerEventData data)
    {
        DataManager.Instance.SaveGameData(SlotIndex);
        RefreshUI();
    }
}

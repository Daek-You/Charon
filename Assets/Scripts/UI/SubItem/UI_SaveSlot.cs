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

        // switch ���� type�� �þ���� ��ȿ�����̹Ƿ� ���ȭ�� �ʿ䰡 ����
        // ������ �ȴٸ� ���ȭ ������ ���ؼ� �����غ� ��
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
            GetText((int)Texts.TxtIndex).text = $"{SlotIndex + 1}�� ������";
            GetText((int)Texts.TxtReinforecInfo).text = MakeReinforceText(slotData);
            GetText((int)Texts.TxtStageInfo).text = MakeStageText();
        }
        else
        {
            GetText((int)Texts.TxtIndex).text = "������ ����";
            GetText((int)Texts.TxtReinforecInfo).text = "";
            GetText((int)Texts.TxtStageInfo).text = "";
        }
    }


    public string MakeReinforceText(GameData slotData)
    {
        // ���� �� ���� ������ ������ ��� �� ������ ȣ�� (�� ���� �� ����)
        // ���⸦ Poolable�� ����ٸ� 2��° ȣ����ʹ� ���ϰ� �پ�� ��
        GameObject go = Utils.Instantiate($"Weapons/{DataManager.Instance.SaveData.WeaponName}");
        string weaponName = go.GetComponent<CharonPaddle>().Name;
        Utils.Destroy(go);

        string text = $"{weaponName} +{DataManager.Instance.SaveData.CurrentWeaponReinforecLevel}\n"
            + $"ü�� +{slotData.CurrentHPReinforceLevel}\n"
            + $"���� +{slotData.CurrentArmorReinforceLevel}\n"
            + $"�̵��ӵ� +{slotData.CurrentMoveSpeedReinforceLevel}\n"
            + $"��� +{slotData.CurrentDashCountReinforceLevel}";
        return text;
    }

    public string MakeStageText()
    {
        string text = "";
        StageType type = DataManager.Instance.SaveData.CurrentStage;

        if (type == StageType.Lobby)
        {
            text = "�κ�";
        }
        else if (type != StageType.Unknown && type != StageType.Ending && type != StageType.Title && type != StageType.Loading && type != StageType.Opening)
        {
            text = DataManager.Instance.SaveData.CurrentStage.ToString();
            text = text.Substring(text.Length - 2);
            text = $"��������\n{text[0]}-{text[1]}";
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

        // �����Ͱ� ���� ���� �ش� ���� �ҷ����� (���� ��� ������)
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

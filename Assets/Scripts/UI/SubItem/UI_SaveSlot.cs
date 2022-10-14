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
        TxtIndex
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
        isExist = slotData.isSaved ? true : false;

        if (isExist)
        {
            GetText((int)Texts.TxtIndex).text = $"{SlotIndex}�� ������";
        }
        else
        {
            GetText((int)Texts.TxtIndex).text = "������ ����";
        }
    }

    public void OnStartNewGame(PointerEventData data)
    {
        DataManager.Instance.DataIndex = SlotIndex;

        if (isExist)
            UIManager.Instance.ShowPopupUI<UI_CheckMessage>();
        else
            UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "LobbyScene");
    }

    public void OnContinueGame(PointerEventData data)
    {
        DataManager.Instance.DataIndex = SlotIndex;

        // �����Ͱ� ���� ���� �ش� ���� �ҷ����� (���� ��� ������)
        if (isExist)
        {
            DataManager.Instance.LoadGameData(SlotIndex);

            // �ҷ��� �������� �������� ������ Ȯ���Ͽ� �̵���ų ���� ����
        }
    }

    public void OnSaveCurrentGame(PointerEventData data)
    {
        DataManager.Instance.SaveGameData(SlotIndex);
        RefreshUI();
    }
}

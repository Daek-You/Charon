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
        isExist = slotData.isSaved ? true : false;

        if (isExist)
        {
            GetText((int)Texts.TxtIndex).text = $"{SlotIndex}번 데이터";
        }
        else
        {
            GetText((int)Texts.TxtIndex).text = "데이터 없음";
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

        // 데이터가 있을 때만 해당 게임 불러오기 (없을 경우 무반응)
        if (isExist)
        {
            DataManager.Instance.LoadGameData(SlotIndex);

            // 불러온 데이터의 스테이지 정보를 확인하여 이동시킬 씬을 결정
        }
    }

    public void OnSaveCurrentGame(PointerEventData data)
    {
        DataManager.Instance.SaveGameData(SlotIndex);
        RefreshUI();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SaveSlot : UI_Base
{
    public UI_ManageData.DataState SlotType { get; set; }
    bool isExist;

    enum Images
    {
        ImgSlot
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
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

        
    }

    public void OnStartNewGame(PointerEventData data)
    {
        if (isExist)
            UIManager.Instance.ShowPopupUI<UI_CheckMessage>();
        else
        {
            Debug.Log("새로하기 (데이터 없음)");
            // 처음부터 게임 시작
        }
    }

    public void OnContinueGame(PointerEventData data)
    {
        if (isExist)
        {
            Debug.Log("불러오기 (데이터 있음)");
            // 데이터가 있을 때만 해당 게임 불러오기 (없을 경우 무반응)
        }
    }

    public void OnSaveCurrentGame(PointerEventData data)
    {
        Debug.Log("저장");
        // 클릭한 데이터 슬롯에 현재 상황 저장
    }
}

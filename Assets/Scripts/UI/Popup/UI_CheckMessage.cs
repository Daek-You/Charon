using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_CheckMessage : UI_Popup
{
    enum GameObjects
    {
        BackgroundPanel
    }

    enum Texts
    {
        TxtMessage,
        TxtStart,
        TxtCancel
    }

    enum Buttons
    {
        BtnStart,
        BtnCancel
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GameObject button = GetButton((int)Buttons.BtnStart).gameObject;
        BindEvent(button, OnDeleteAndNewStart, UIEvent.Click);
        button = GetButton((int)Buttons.BtnCancel).gameObject;
        BindEvent(button, OnDeleteCancle, UIEvent.Click);
    }

    public void OnDeleteAndNewStart(PointerEventData data)
    {
        // 기존 데이터 삭제 작업이 필요
        Debug.Log("데이터 지우고 새로하기");

        // SceneManager.LoadScene();
    }

    public void OnDeleteCancle(PointerEventData data)
    {
        UIManager.Instance.ClosePopupUI();
    }
}

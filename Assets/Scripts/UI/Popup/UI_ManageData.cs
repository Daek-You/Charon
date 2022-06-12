using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_ManageData : UI_Popup
{
    protected enum GameObjects
    {
        BackgroundPanel,
        GridPanel
    }

    protected enum Texts
    {
        TxtStartGame,
        TxtReturn
    }

    protected enum Buttons
    {
        BtnReturn
    }

    public enum DataState
    {
        Unknown,
        NewGame,
        Continue,
        SaveGame
    }

    protected int numOfSlot = 4;
    GameObject panel;

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GameObject button = GetButton((int)Buttons.BtnReturn).gameObject;
        BindEvent(button, OnReturn, UIEvent.Click);

        // 패널 초기화
        panel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach (Transform child in panel.transform)
            Utils.Destroy(child.gameObject);
    }

    public void OnReturn(PointerEventData data)
    {
        ClosePopupUI();
    }

    public void SetNamespace(string name)
    {
        GetText((int)Texts.TxtStartGame).text = name;
    }

    public void SetSaveSlot(DataState type)
    {
        for (int i = 0; i < numOfSlot; i++)
        {
            GameObject slot = Utils.Instantiate("UI/SubItem/UI_SaveSlot");
            slot.transform.SetParent(panel.transform);

            UI_SaveSlot saveSlot = Utils.GetAddedComponent<UI_SaveSlot>(slot);
            saveSlot.SlotType = type;

            // saveSlot에 i번째 세이브 데이터를 할당
            // 새로하기에서 이미 존재하는 데이터를 누를 경우 경고 메시지
            // 이어하기에서 데이터가 없는 슬롯을 누를 경우 무반응

            BindEvent(saveSlot.gameObject, OnStartGame, UIEvent.Click);
        }
    }

    public void OnStartGame(PointerEventData data)
    {
        SceneManager.LoadScene("UITestScene");
    }
}

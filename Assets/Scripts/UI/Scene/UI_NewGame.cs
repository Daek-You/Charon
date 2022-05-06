using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_NewGame : UI_Scene
{
    enum GameObjects
    {
        BackgroundPanel,
        GridPanel
    }

    enum Texts
    {
        TxtStartGame,
        TxtReturn
    }

    enum Buttons
    {
        BtnReturn
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

        GetText((int)Texts.TxtStartGame).text = "�����ϱ�";
        GameObject button = GetButton((int)Buttons.BtnReturn).gameObject;
        BindEvent(button, OnReturn, UIEvent.Click);

        // �г� �ʱ�ȭ
        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach (Transform child in gridPanel.transform)
            Utils.Destroy(child.gameObject);

        for (int i = 0; i < 4; i++)
        {
            GameObject slot = Utils.Instantiate("UI/SubItem/UI_SaveSlot");
            slot.transform.SetParent(gridPanel.transform);

            UI_SaveSlot saveSlot = Utils.GetAddedComponent<UI_SaveSlot>(slot);

            // saveSlot�� i��° ���̺� �����͸� �Ҵ�
            // ����� Ȯ�� â �׽�Ʈ�� ���� �ӽ� ���� �������
            saveSlot.ChangeSaveData(i);
        }
    }

    public void OnReturn(PointerEventData data)
    {
        UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "TitleScene");
    }
}

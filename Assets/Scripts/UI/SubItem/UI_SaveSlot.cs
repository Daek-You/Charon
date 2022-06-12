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

        
    }

    public void OnStartNewGame(PointerEventData data)
    {
        if (isExist)
            UIManager.Instance.ShowPopupUI<UI_CheckMessage>();
        else
        {
            Debug.Log("�����ϱ� (������ ����)");
            // ó������ ���� ����
        }
    }

    public void OnContinueGame(PointerEventData data)
    {
        if (isExist)
        {
            Debug.Log("�ҷ����� (������ ����)");
            // �����Ͱ� ���� ���� �ش� ���� �ҷ����� (���� ��� ������)
        }
    }

    public void OnSaveCurrentGame(PointerEventData data)
    {
        Debug.Log("����");
        // Ŭ���� ������ ���Կ� ���� ��Ȳ ����
    }
}

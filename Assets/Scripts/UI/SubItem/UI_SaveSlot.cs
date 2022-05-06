using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SaveSlot : UI_Base
{
    bool isExist = false;

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
        BindEvent(image, OnDeleteSaveData, UIEvent.Click);
    }

    public void OnDeleteSaveData(PointerEventData data)
    {
        if (isExist)
            UIManager.Instance.ShowPopupUI<UI_CheckMessage>();
    }

    public void ChangeSaveData(int temp)
    {
        if (temp % 2 == 0)
            isExist = true;
        else
            isExist = false;
    }
}

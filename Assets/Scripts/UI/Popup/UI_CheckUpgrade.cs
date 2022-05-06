using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CheckUpgrade : UI_Popup
{
    enum GameObjects
    {
        GridPanel
    }

    static bool isOpen = false;
    public static bool IsOpen { get { return isOpen; } }

    void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        isOpen = false;
    }

    public override void Init()
    {
        base.Init();

        isOpen = true;
        Bind<GameObject>(typeof(GameObjects));

        GameObject panel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach (Transform child in panel.transform)
            Utils.Destroy(child.gameObject);

        // 내용물을 추가하는 기능
    }
}

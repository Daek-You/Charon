using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Interaction3D : UI_Base
{
    enum Texts
    {
        TextTarget,
        TextInteraction
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ShowUI, OnShowUI);
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.HideUI, OnHideUI);

        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);

        gameObject.SetActive(false);
    }

    public void OnShowUI(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {

        if (!transform.parent.name.Equals((string)param))
            return;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }

    public void OnHideUI(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        if (transform.parent.name.Equals((string)param))
            return;

        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public abstract class BaseScene : MonoBehaviour
{
    public abstract void Init();
    public abstract void Clear();

    public void CreateEventSystem()
    {
        Object eventSystem = GameObject.FindObjectOfType(typeof(EventSystem));
        if (eventSystem == null)
            Utils.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public void OnChangeScene(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        Clear();
        SceneManager.LoadScene((string)param);
    }
}

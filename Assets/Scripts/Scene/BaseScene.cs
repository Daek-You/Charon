using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseScene : MonoBehaviour
{
    public abstract void Init();
    public abstract void Clear();

    public void OnChangeScene(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        SceneManager.LoadScene((string)param);
        Clear();
    }
}

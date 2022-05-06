using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        Init();
    }

    public void Init()
    {
        UIManager.Instance.ShowSceneUI<UI_InGame>();
    }
}

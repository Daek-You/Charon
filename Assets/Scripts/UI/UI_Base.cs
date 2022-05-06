using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public abstract class UI_Base : MonoBehaviour
{
    public enum UIEvent
    {
        Click,
        Enter,
        Exit,
        Up
    }

    Dictionary<Type, UnityEngine.Object[]> objectDictionary = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        objectDictionary.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Utils.FindChild(gameObject, names[i], true);
            else
                objects[i] = Utils.FindChild<T>(gameObject, names[i], true);
        }
    }

    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (objectDictionary.TryGetValue(typeof(T), out objects) == false)
            return null;
        return objects[index] as T;
    }

    protected GameObject GetObject(int index) { return Get<GameObject>(index); }
    protected TextMeshProUGUI GetText(int index) { return Get<TextMeshProUGUI>(index); }
    protected Button GetButton(int index) { return Get<Button>(index); }
    protected Image GetImage(int index) { return Get<Image>(index); }
    protected Toggle GetToggle(int index) { return Get<Toggle>(index); }

    public static void BindEvent(GameObject uiObject, Action<PointerEventData> action, UIEvent type = UIEvent.Click)
    {
        UI_PointerEventHandler evt = Utils.GetAddedComponent<UI_PointerEventHandler>(uiObject);

        switch (type)
        {
            case UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case UIEvent.Enter:
                evt.OnEnterHandler -= action;
                evt.OnEnterHandler += action;
                break;
            case UIEvent.Exit:
                evt.OnExitHandler -= action;
                evt.OnExitHandler += action;
                break;
            case UIEvent.Up:
                evt.OnUpHandler -= action;
                evt.OnUpHandler += action;
                break;
        }
    }
}

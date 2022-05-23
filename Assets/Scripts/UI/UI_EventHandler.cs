using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EventHandler
{
    public enum UIEventType
    {
        ChangeWeapon,
        ChangeHP,
        ChangeST,
        ChangeGoods,
        ChangeScene,
        NextDialogue,
        ChangeStat
    }

    public Action<UIEventType, Component, object> OnUIEvent = null;
    Dictionary<UIEventType, List<Action<UIEventType, Component, object>>> listeners = new Dictionary<UIEventType, List<Action<UIEventType, Component, object>>>();

    public void AddListener(UIEventType eventType, Action<UIEventType, Component, object> action)
    {
        List<Action<UIEventType, Component, object>> listenList = null;

        if (listeners.TryGetValue(eventType, out listenList))
        {
            listenList.Add(action);
            return;
        }

        listenList = new List<Action<UIEventType, Component, object>>();
        listenList.Add(action);
        listeners.Add(eventType, listenList);
    }

    public void PostNotification(UIEventType eventType, Component sender, object param = null)
    {
        List<Action<UIEventType, Component, object>> listenList = null;

        if (!listeners.TryGetValue(eventType, out listenList))
            return;

        for (int i = 0; i < listenList.Count; i++)
        {
            if (!listenList[i].Equals(null))
                listenList[i](eventType, sender, param);
        }
    }

    public void RemoveEvent(UIEventType eventType)
    {
        listeners.Remove(eventType);
    }

    public void RemoveRedundancies()
    {
        Dictionary<UIEventType, List<Action<UIEventType, Component, object>>> tmpListeners = new Dictionary<UIEventType, List<Action<UIEventType, Component, object>>>();

        foreach (KeyValuePair<UIEventType, List<Action<UIEventType, Component, object>>> item in listeners)
        {
            for (int i = item.Value.Count - 1; i >= 0; i--)
            {
                if (item.Value[i].Equals(null))
                    item.Value.RemoveAt(i);
            }

            if (item.Value.Count > 0)
                tmpListeners.Add(item.Key, item.Value);
        }

        listeners = tmpListeners;
    }

    public void Clear()
    {
        RemoveRedundancies();
    }
}

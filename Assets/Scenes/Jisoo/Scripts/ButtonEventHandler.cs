using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEventHandler
{
    public enum BTNEventType
    {
        UpgradeMaxHP,
        UpgradeArmor,
        UpgradeSpeed,
        UpgradeDash,
        UpgradePaddle,
        UpgradeSickle,
        UpgradeGourd
    }

    //public GameObject[] buttons = new GameObject[4];
    

    //public Action<UIEventType, Component, object> OnUIEvent = null;
    //Dictionary<UIEventType, List<Action<UIEventType, Component, object>>> listeners = new Dictionary<UIEventType, List<Action<UIEventType, Component, object>>>();


    //public void PostNotification(UIEventType eventType, Component sender, object param = null)
    //{
    //    List<Action<UIEventType, Component, object>> listenList = null;

    //    if (!listeners.TryGetValue(eventType, out listenList))
    //        return;

    //    for (int i = 0; i < listenList.Count; i++)
    //    {
    //        if (!listenList[i].Equals(null))
    //            listenList[i](eventType, sender, param);
    //    }
    //}
}

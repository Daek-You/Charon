using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Sondol"))
            return;

        UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.AccessStage, this);
        Utils.Destroy(gameObject);
    }
}

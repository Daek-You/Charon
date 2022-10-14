using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportTemp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sondol"))
        {
            UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "Stage1Scene");
        }
    }
}

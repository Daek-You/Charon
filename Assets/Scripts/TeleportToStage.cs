using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToStage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sondol"))
        {
            FadeInOutController.Instance.FadeOutAndLoadScene("Stage1Scene_DaekYou", StageType.Stage11);
        }
            //UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "Stage1Scene");
    }
}

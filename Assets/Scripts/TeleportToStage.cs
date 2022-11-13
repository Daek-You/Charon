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
            // FadeInOutController.Instance.FadeOutAndLoadScene("Stage1Scene", StageType.Stage11);
            FadeInOutController.Instance.FadeOutAndLoadScene("StageTestScene", StageType.Stage11);
        }
            //UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "Stage1Scene");
    }
}

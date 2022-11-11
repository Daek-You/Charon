using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LogoFadeInAndOut : MonoBehaviour
{
    public RawImage logo;
    private WaitForSeconds delay = new WaitForSeconds(0.005f);

    private void Start()
    {
        StartCoroutine("Fade");
    }

    IEnumerator Fade()
    {
        float fadeCount = 0f;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.005f;
            yield return delay;
            logo.color = new Color(255, 255, 255, fadeCount);
        }

        yield return delay;

        while (fadeCount > 0f)
        {
            fadeCount -= 0.005f;
            yield return delay;
            logo.color = new Color(255, 255, 255, fadeCount);
        }

        UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "TitleScene");
        BGM_Manager.Instance.Play(StageType.Title);
    }
}

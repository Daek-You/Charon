using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LogoFadeInAndOut : MonoBehaviour
{
    public RawImage logo;
    private void Start()
    {
        StartCoroutine("FadeIn");
    }
    IEnumerator FadeIn()
    {
        float fadeCount = 0f;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.005f;
            yield return new WaitForSeconds(0.005f);
            logo.color = new Color(255, 255, 255, fadeCount);
        }
        yield return new WaitForSeconds(2f);
        UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "TitleScene");
    }
}

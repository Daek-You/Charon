using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Logo : UI_Popup
{
    static bool escLock = false;
    public static bool EscLock { get { return escLock; } }

    enum Images
    {
        ImgLogo
    }

    void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        escLock = false;
    }

    public override void Init()
    {
        base.Init();

        escLock = true;

        Bind<Image>(typeof(Images));
        StartCoroutine("CorFadeAnimation", 4.0f);
    }

    IEnumerator CorFadeAnimation(float second)
    { 
        Animator anim = GetImage((int)Images.ImgLogo).GetComponent<Animator>();
        yield return new WaitForSeconds(second);

        anim.SetBool("IsFadeOut", true);
        yield return new WaitForSeconds(2.0f);

        UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "TitleScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ending : UI_Scene
{
    enum Images
    {
        ImgFirst,
        ImgSecond
    }

    enum Texts
    {
        TxtEnding
    }

    float ACTIVE_TIME = 5.0f;
    float FADE_TIME = 0.002f;

    void Start()
    {
        Init();

        StartCoroutine("CorCooldown", ACTIVE_TIME);
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        DataManager.Instance.IsDirOrClear = true;
    }

    IEnumerator CorCooldown(float second)
    {
        float cool = second;
        while (cool > 0)
        {
            cool -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        if (GetImage((int)Images.ImgFirst).gameObject.activeSelf)
        {
            StartCoroutine("FadeImage");
            StartCoroutine("FadeText");
        }
        else
            FadeInOutController.Instance.FadeOutAndLoadScene("LobbyScene", StageType.Lobby);
    }

    public IEnumerator FadeImage()
    {
        for (float f = 1f; f > 0f; f -= FADE_TIME)
        {
            Color color = GetImage((int)Images.ImgFirst).GetComponent<Image>().color;
            color.a = f;
            GetImage((int)Images.ImgFirst).GetComponent<Image>().color = color;
            yield return null;
        }

        GetImage((int)Images.ImgFirst).gameObject.SetActive(false);
        StartCoroutine("CorCooldown", ACTIVE_TIME);
    }

    public IEnumerator FadeText()
    {
        for (float f = 1f; f > 0f; f -= FADE_TIME)
        {
            Color color = GetText((int)Texts.TxtEnding).GetComponent<TextMeshProUGUI>().color;
            color.a = f;
            GetText((int)Texts.TxtEnding).GetComponent<TextMeshProUGUI>().color = color;
            yield return null;
        }

        GetText((int)Texts.TxtEnding).gameObject.SetActive(false);
    }
}

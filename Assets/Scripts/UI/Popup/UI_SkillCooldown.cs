using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_SkillCooldown : UI_Popup
{
    static bool isCooldown = false;
    public static bool IsCooldown { get { return isCooldown; } }

    enum Texts
    {
        TxtCooldown
    }

    enum Images
    {
        ImgCooldownFrame
    }

    private void Awake()
    {
        Init();
    }

    void OnEnable()
    {
        StartCooldown();
    }

    private void OnDisable()
    {
        isCooldown = false;
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    public void StartCooldown()
    {
        isCooldown = true;

        // 현재 무기의 스킬 쿨타임 정보를 받아와야함
        float cool = Random.Range(3, 6);
        StartCoroutine("CorCooldown", cool);
        StartCoroutine("CorFrameCooldown", cool);
    }

    // 스킬 쿨타임의 남은 시간을 출력하는 함수
    IEnumerator CorCooldown (float second)
    {
        var wait = new WaitForSeconds(1);
        while (second > 0.0f)
        {
            GetText((int)Texts.TxtCooldown).text = second.ToString();
            second--;
            yield return wait;
        }

        Utils.Destroy(gameObject);
    }

    // 스킬 쿨타임의 남은 시간을 시각적으로 보여주는 함수
    IEnumerator CorFrameCooldown (float second)
    {
        float cool = second;
        while (cool > 0)
        {
            cool -= Time.deltaTime;
            GetImage((int)Images.ImgCooldownFrame).fillAmount = cool / second;
            yield return new WaitForFixedUpdate();
        }
    }
}

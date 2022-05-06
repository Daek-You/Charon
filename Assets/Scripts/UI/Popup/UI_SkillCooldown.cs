using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SkillCooldown : UI_Popup
{
    static bool isCooldown = false;
    public static bool IsCooldown { get { return isCooldown; } }

    enum Texts
    {
        TxtCooldown
    }

    void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        isCooldown = false;
    }

    public override void Init()
    {
        base.Init();

        isCooldown = true;
        Bind<TextMeshProUGUI>(typeof(Texts));

        // 현재 무기의 스킬 쿨타임 정보를 받아와야함
        float cool = Random.Range(3, 6);
        StartCoroutine("CorCooldown", cool);
    }

    IEnumerator CorCooldown (float second)
    {
        while (second > 0.0f)
        {
            GetText((int)Texts.TxtCooldown).text = second.ToString();
            second--;
            yield return new WaitForSeconds(1);
        }

        Utils.Destroy(gameObject);
    }
}

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

        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeSuccess, OnUseSkill);
    }

    public void StartCooldown()
    {
        isCooldown = true;
        UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeSuccess, this);

        // ���� ������ ��ų ��Ÿ�� ������ �޾ƿ;���
        float cool = Random.Range(3, 6);
        StartCoroutine("CorCooldown", cool);
        StartCoroutine("CorFrameCooldown", cool);
    }

    public void OnUseSkill(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        QuestReporter reporter = Utils.GetAddedComponent<QuestReporter>(this.gameObject);
        reporter.Report();
    }

    // ��ų ��Ÿ���� ���� �ð��� ����ϴ� �Լ�
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

    // ��ų ��Ÿ���� ���� �ð��� �ð������� �����ִ� �Լ�
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
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatSlot : UI_Base
{
    enum Images
    {
        ImgStat
    }

    enum Texts
    {
        TxtUpgradeCount
    }

    private void Awake()
    {
        Init();
    }

    void OnEnable()
    {
        // �� ���ȿ� ���� �ʱ�ȭ�� ���⼭ �� �� ��
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // ���� ���¿��� ��ȭ ��ġ�� ���� ��� �̺�Ʈ�� �����ϵ��� �߰�
        // �� ���Ⱥ��� �̺�Ʈ�� �߰� -> �̺�Ʈ ������ ������ (�� ���� �׳��� ���� �� ����)
    }
}

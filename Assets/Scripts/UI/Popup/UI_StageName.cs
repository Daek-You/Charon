using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StageName : UI_Popup
{
    enum Texts
    {
        TxtStageName
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));

        // UI�� ǥ�õ� ���������� ����
        // �ڷ�ƾ���� N�ʰ� ǥ�� �� Destroy
        // �ִϸ��̼� ȿ�� �ʿ�
    }
}

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

        // UI에 표시될 스테이지명 설정
        // 코루틴으로 N초간 표시 후 Destroy
        // 애니메이션 효과 필요
    }
}

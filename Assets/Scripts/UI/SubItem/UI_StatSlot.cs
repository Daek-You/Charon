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
        // 각 스탯에 대한 초기화를 여기서 해 줄 것
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // 열린 상태에서 강화 수치가 변할 경우 이벤트를 실행하도록 추가
        // 각 스탯별로 이벤트를 추가 -> 이벤트 개수가 많아짐 (이 쪽이 그나마 나은 거 같음)
    }
}

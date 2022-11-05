using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : Interactable
{
    private void Start()
    {
        interactionText = "큐브강화";
    }

    public override void interact()
    {
        // 상속 받는 클래스마다 다르며, UI를 열거나 무기를 변경하는 등의 동작이 있을 수 있음
        interactionText = $"큐브강화 성공";
    }
}

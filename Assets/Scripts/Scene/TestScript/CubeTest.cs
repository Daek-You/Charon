using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : Interactable
{
    private void Start()
    {
        interactionText = "ť�갭ȭ";
    }

    public override void interact()
    {
        // ��� �޴� Ŭ�������� �ٸ���, UI�� ���ų� ���⸦ �����ϴ� ���� ������ ���� �� ����
        interactionText = $"ť�갭ȭ ����";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : Interactable
{
    private void Start()
    {
        UIManager.Instance.MakeWorldSpaceUI<UI_Interaction3D>(transform);
    }

    public override void interact()
    {
        // ��� �޴� Ŭ�������� �ٸ���, UI�� ���ų� ���⸦ �����ϴ� ���� ������ ���� �� ����
        Debug.Log(gameObject.name);
    }
}

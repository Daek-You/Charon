using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public int talkIndex;
    public Text easyTalk;
    public GameObject scanObject;
    public bool isAction;
    public GameObject talkPanel;

    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjData A= scanObject.GetComponent<ObjData>();
        Talk(A.id, A.isNPC);
        talkPanel.SetActive(isAction);
    }



    //���� ������ UI�� ����ϴ� �Լ�     
    void Talk(int id, bool isNPC)
    {
        string talkData = talkManager.GetTalk(id, talkIndex); //�ش��ϴ� ���ڿ��� ���´�. 

        if (talkData == null) { isAction = false; talkIndex = 0; return; } // �̾߱Ⱑ �� ������, �� �ε����� �� ���ư��� ��ȭâ ������, void�Լ����� return�� ���� ���� ����. 


        if (isNPC) //é�ͺ� ���� ���θ��� id���� 
        {
            easyTalk.text = talkData;
        }

        else
        {
            easyTalk.text = talkData;
        }
        isAction = true;
        talkIndex++;
    }
}


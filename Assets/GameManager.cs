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



    //실제 대사들을 UI에 출력하는 함수     
    void Talk(int id, bool isNPC)
    {
        string talkData = talkManager.GetTalk(id, talkIndex); //해당하는 문자열이 나온다. 

        if (talkData == null) { isAction = false; talkIndex = 0; return; } // 이야기가 다 끝나고, 즉 인덱스가 다 돌아가면 대화창 내리기, void함수에서 return은 강제 종료 역할. 


        if (isNPC) //챕터별 상점 주인마다 id지정 
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


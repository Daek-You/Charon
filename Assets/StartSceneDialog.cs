using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneDialog : MonoBehaviour
{

    public int id;
    public GameObject talkPanel;
    public Text easyTalk;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;
    Dictionary<int, string[]> talkData;

    public void Start()
    {


        //action할떄마다 문장을 count한 뒤 대화의 마지막 값이 나오면 대화를 setactive (false)한다. 그리고 씬 전환을 한다?

    }
}
    
    
    /*
    public void Action()
    {
    Talk(id);
        talkPanel.SetActive(isAction);
    }

    void Talk(int id) {
        string talkData = GetTalk( id, talkIndex);
        if (talkData == null) { isAction = false; talkIndex = 0; return; }
        easyTalk.text = talkData;
        isAction = true;
        talkIndex++;
    }

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenarateData();
    }

    void GenarateData()
    {
        talkData.Add(1, new string[] {"여기는 어디요?" , "저승으로 가는 강이요." , "제가 죽은 거요? 전하! 전하께서는…?" , "그래 너는 죽었다.", "다른 이는 모른다. 난 그저 뱃사공일 뿐이다.", "그렇군요…. 다시 이승으로 갈 수 있는 방법은 없는 건가요?  작은 일만 다시하고 오겠소… 살려주시오.", "음,,, 원래라면 안된다. 하지만, 당신에게서 에우로스님의 기운이 느껴지는 군. 하나의 부탁만 들어준다면 기꺼이 도와주지." , "무엇이든 하겠습니다. 도와주십시오.", "이 배에서 내리거든 노를 잠깐 잡아주게. 그것이 내 부탁이라네." });
       
    }

    public string GetTalk (int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
    */
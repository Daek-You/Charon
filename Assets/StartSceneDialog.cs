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


        //action�ҋ����� ������ count�� �� ��ȭ�� ������ ���� ������ ��ȭ�� setactive (false)�Ѵ�. �׸��� �� ��ȯ�� �Ѵ�?

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
        talkData.Add(1, new string[] {"����� ����?" , "�������� ���� ���̿�." , "���� ���� �ſ�? ����! ���ϲ����¡�?" , "�׷� �ʴ� �׾���.", "�ٸ� �̴� �𸥴�. �� ���� ������ ���̴�.", "�׷����䡦. �ٽ� �̽����� �� �� �ִ� ����� ���� �ǰ���?  ���� �ϸ� �ٽ��ϰ� ���ڼҡ� ����ֽÿ�.", "��,,, ������� �ȵȴ�. ������, ��ſ��Լ� ����ν����� ����� �������� ��. �ϳ��� ��Ź�� ����شٸ� �Ⲩ�� ��������." , "�����̵� �ϰڽ��ϴ�. �����ֽʽÿ�.", "�� �迡�� �����ŵ� �븦 ��� ����ְ�. �װ��� �� ��Ź�̶��." });
       
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    // Start is called before the first frame update
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenarateData();
    }

    void GenarateData()
    {
        talkData.Add(1, new string[] { "����� ����?", "�������� ���� ���̿�.", "���� ���� �ſ�? ����! ���ϲ����¡�?", "�׷� �ʴ� �׾���.", "�ٸ� �̴� �𸥴�. �� ���� ������ ���̴�.", "�׷����䡦. �ٽ� �̽����� �� �� �ִ� ����� ���� �ǰ���?  ���� �ϸ� �ٽ��ϰ� ���ڼҡ� ����ֽÿ�.", "��,,, ������� �ȵȴ�. ������, ��ſ��Լ� ����ν����� ����� �������� ��. �ϳ��� ��Ź�� ����شٸ� �Ⲩ�� ��������.", "�����̵� �ϰڽ��ϴ�. �����ֽʽÿ�.", "�� �迡�� �����ŵ� �븦 ��� ����ְ�. �װ��� �� ��Ź�̶��." });

    }

    public string GetTalk(int id, int talkIndex) //Object�� id , string�迭�� index
    {
        if (talkIndex == talkData[id].Length) //�ش� id�� ������ string�迭�� ���̿� ���� 
            return null;
        else
            return talkData[id][talkIndex]; //�ش� ���̵��� �ش��ϴ� ��縦 ��ȯ 
    }
}
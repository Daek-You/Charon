using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LitJson;
public class OpeningManager : MonoBehaviour
{
    //public TextMesh nameText;       //�̸� ���� �ؽ�Ʈ �ڽ�
    public TMP_Text dialogText;     //��ȭ ���� �ؽ�Ʈ �ڽ�
    
    void Start()
    {
        var jsonTextFile = Resources.Load("Json/DialogList");
        if (!jsonTextFile)
            Debug.Log("���Ͼ���");
        JsonData jsonData = JsonMapper.ToObject(jsonTextFile.ToString());
        dialogText.text = jsonData[0][0][0].ToString();
        Debug.Log(jsonData);
        //for(int i = 0; i < myData.Count; i++)
        //{
           

        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

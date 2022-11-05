using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LitJson;
public class OpeningManager : MonoBehaviour
{
    //public TextMesh nameText;       //이름 들어가는 텍스트 박스
    public TMP_Text dialogText;     //대화 들어가는 텍스트 박스
    
    void Start()
    {
        var jsonTextFile = Resources.Load("Json/DialogList");
        if (!jsonTextFile)
            Debug.Log("파일없음");
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

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Dialogue
{
    public int id;
    public string name;
    public string description;
}

[Serializable]
public class DialogueData
{
    public List<Dialogue> dialogues = new List<Dialogue>();
}

public class UI_Dialogue : UI_Popup
{
    public Dictionary<int, Dialogue> DialogueDictionary { get; private set; } = new Dictionary<int, Dialogue>();

    static bool isDialogue = false;
    public static bool IsDialogue { get { return isDialogue; } }

    int dialogueCount = 0;

    enum Images
    {
        ImgBackground,
        ImgNamespace
    }

    enum Texts
    {
        TxtDialogue,
        TxtName
    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        UIManager.EventHandler.RemoveEvent(UI_EventHandler.UIEventType.NextDialogue);
        dialogueCount = 0;
        isDialogue = false;
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        TextAsset textAsset = Utils.Load<TextAsset>("DataTest/DialogueData");
        DialogueData data = JsonUtility.FromJson<DialogueData>(textAsset.text); // ���Ⱑ ����
        foreach (Dialogue dialogue in data.dialogues)
            DialogueDictionary.Add(dialogue.id, dialogue);

        ShowDialogue();
    }

    public void ShowDialogue()
    {
        GetText((int)Texts.TxtName).text = DialogueDictionary[dialogueCount].name;
        GetText((int)Texts.TxtDialogue).text = DialogueDictionary[dialogueCount].description;

        isDialogue = true;
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.NextDialogue, OnNextDialogue);
    }

    public void OnNextDialogue(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        dialogueCount++;
        if (dialogueCount >= DialogueDictionary.Count)
        {
            // ����� ��ȭ�� length���� Ŭ ��� ����
            UIManager.Instance.ClosePopupUI();
        }
        else
        {
            // �ؽ�Ʈ�� ���� ��ȭ �������� ����
            GetText((int)Texts.TxtName).text = DialogueDictionary[dialogueCount].name;
            GetText((int)Texts.TxtDialogue).text = DialogueDictionary[dialogueCount].description;
        }
    }
}

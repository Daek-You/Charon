using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField]
    private Quest quest;

    public void SetQuest(Quest quest)
    {
        this.quest = quest;
        Register();
    }

    public void Register()
    {
        if (!QuestManager.Instance.ContainsInCompletedQuests(quest) && !QuestManager.Instance.ContainsInActiveQuests(quest))
            QuestManager.Instance.Register(quest);
    }
}

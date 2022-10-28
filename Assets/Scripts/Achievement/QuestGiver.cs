using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField]
    private Quest giverQuest;
    public Quest GiverQuest { get { return giverQuest; }}

    public void Register(Quest quest)
    {
        if (!QuestManager.Instance.ContainsInCompletedQuests(quest) && !QuestManager.Instance.ContainsInActiveQuests(quest))
            giverQuest = QuestManager.Instance.Register(quest);
    }

    public void Complete()
    {
        if (giverQuest != null)
            giverQuest.Complete();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestReporter : MonoBehaviour
{
    [SerializeField]
    private Category category;
    [SerializeField]
    private TaskTarget target;
    [SerializeField]
    private int successCount;

    public void Report()
    {
        QuestManager.Instance.ReceiveReport(category, target, successCount);
    }

    public void SetReporter(Category category, TaskTarget target, int successCount)
    {
        this.category = category;
        this.target = target;
        this.successCount = successCount;
    }
}

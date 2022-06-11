using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField]
    private Achievement[] achievements;

    private void Start()
    {
        foreach (var achievement in achievements)
        {
            if (!AchievementManager.Instance.ContainsInCompletedAchievements(achievement))
                AchievementManager.Instance.Register(achievement);
        }
    }
}

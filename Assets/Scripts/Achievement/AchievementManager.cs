using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    static AchievementManager instance;
    public static AchievementManager Instance { get { Init(); return instance; } }

    public delegate void AchievementRegisteredHandler(Achievement newAchievement);
    public delegate void AchievementCompletedHandler(Achievement achievement);

    public event AchievementRegisteredHandler onAchievementRegistered;
    public event AchievementCompletedHandler onAchievementCompleted;

    private List<Achievement> activeAchivements = new List<Achievement>();
    private List<Achievement> completedAchivements = new List<Achievement>();

    private AchievementDatabase achievementDatabase;

    public IReadOnlyList<Achievement> ActiveAchivements => activeAchivements;
    public IReadOnlyList<Achievement> CompletedAchivements => completedAchivements;

    private void Start()
    {
        Init();

        achievementDatabase = Resources.Load<AchievementDatabase>("Achievement/AchievementDatabase");

        foreach (var achievement in achievementDatabase.Achievemnets)
            Register(achievement);
    }

    static void Init()
    {
        if (instance == null)
        {
            GameObject achieveManager = GameObject.Find("@Achievement_Manager");

            if (achieveManager == null)
            {
                achieveManager = new GameObject("@Achievement_Manager");
                achieveManager.AddComponent<AchievementManager>();
            }

            DontDestroyOnLoad(achieveManager);
            instance = achieveManager.GetComponent<AchievementManager>();
        }
    }

    public Achievement Register(Achievement achievement)
    {
        var newAchievement = achievement.Clone();

        newAchievement.onComplete += OnAchievementCompleted;

        activeAchivements.Add(newAchievement);
        newAchievement.OnRegister();
        onAchievementRegistered?.Invoke(newAchievement);

        return newAchievement;
    }

    private void ReceiveReport(List<Achievement> achievements, string category, object target, int successCount)
    {
        foreach (var achievement in achievements.ToArray())
            achievement.ReceiveReport(category, target, successCount);
    }

    public void ReceiveReport(string category, object target, int successCount)
    {
        ReceiveReport(activeAchivements, category, target, successCount);
    }

    public void ReceiveReport(Category category, TaskTarget target, int successCount)
        => ReceiveReport(category.CodeName, target.Value, successCount);

    public bool ContainsInActiveAchievements(Achievement achievement)
        => activeAchivements.Any(x => x.CodeName == achievement.CodeName);

    public bool ContainsInCompletedAchievements(Achievement achievement)
        => completedAchivements.Any(x => x.CodeName == achievement.CodeName);

    private void OnAchievementCompleted(Achievement achievement)
    {
        activeAchivements.Remove(achievement);
        completedAchivements.Add(achievement);

        onAchievementCompleted?.Invoke(achievement);
    }
}

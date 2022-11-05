using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_AchievementCompletionNotifier : UI_Scene
{
    private float showTime = 3.0f;
    private Queue<Quest> reservedAchievements = new Queue<Quest>();

    enum Texts
    {
        TxtAchievement,
        TxtComplete
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));

        var achieveManager = QuestManager.Instance;
        achieveManager.onAchievementCompleted += Notify;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        var achieveManager = QuestManager.Instance;
        if (achieveManager != null)
            achieveManager.onAchievementCompleted -= Notify;
    }

    private void Notify(Quest achievement)
    {
        reservedAchievements.Enqueue(achievement);

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            StartCoroutine("CorShowNotice");
        }
    }

    private IEnumerator CorShowNotice()
    {
        var waitSeconds = new WaitForSeconds(showTime);

        Quest achievement;
        while (reservedAchievements.TryDequeue(out achievement))
        {
            GetText((int)Texts.TxtAchievement).text = achievement.DisplayName;
            yield return waitSeconds;
        }

        // Destroy°¡ ¸Â³ª?
        Utils.Destroy(gameObject);
    }
}

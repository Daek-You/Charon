using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AchievementView : UI_Popup
{
    static bool isAchievementOpen = false;
    public static bool IsAchievementOpen { get { return isAchievementOpen; } }

    enum GameObjects
    {
        BackgroundPanel,
        GridPanel
    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        isAchievementOpen = false;
    }

    public override void Init()
    {
        base.Init();

        isAchievementOpen = true;

        Bind<GameObject>(typeof(GameObjects));

        GameObject panel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach (Transform child in panel.transform)
            Utils.Destroy(child.gameObject);

        var achieveManager = AchievementManager.Instance;
        CreateDetailViews(achieveManager.ActiveAchivements, panel);
        CreateDetailViews(achieveManager.CompletedAchivements, panel);
    }

    private void CreateDetailViews(IReadOnlyList<Achievement> achievements, GameObject parent)
    {
        foreach (var achievement in achievements)
        {
            GameObject detail = Utils.Instantiate("UI/SubItem/UI_AchievementDetailView");
            detail.transform.SetParent(parent.transform);

            UI_AchievementDetailView detailSlot = Utils.GetAddedComponent<UI_AchievementDetailView>(detail);
            detailSlot.Setup(achievement);
        }
    }
}

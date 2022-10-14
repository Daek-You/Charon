using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AchievementDetailView : UI_Base
{
    private Quest target;

    enum GameObjects
    {
        ImgComplete
    }

    enum Texts
    {
        TxtAchievementName
    }

    enum Buttons
    {
        BtnAchievementDetails
    }

    private void OnDestroy()
    {
        if (target != null)
            target.onCompleted -= ShowCompletionScreen;
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GameObject go = GetButton((int)Buttons.BtnAchievementDetails).gameObject;
        BindEvent(go, onClickAchievementName, UIEvent.Click);

        GetObject((int)GameObjects.ImgComplete).SetActive(false);
    }

    public void Setup(Quest achievement)
    {
        Init();

        target = achievement;
        GetText((int)Texts.TxtAchievementName).text = achievement.DisplayName;

        if (achievement.IsCompleted)
        {
            GetObject((int)GameObjects.ImgComplete).SetActive(true);
        }
        else
        {
            GetObject((int)GameObjects.ImgComplete).SetActive(false);
            achievement.onCompleted += ShowCompletionScreen;
            // achievement.onTaskSuccessChanged += UpdateDescription;
        }
    }

    private void ShowCompletionScreen(Quest achievement)
        => GetObject((int)GameObjects.ImgComplete).SetActive(true);

    public void onClickAchievementName(PointerEventData data)
    {
        UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.CheckAchievement, this, target);
    }
}

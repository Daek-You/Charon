using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (applicationQuitting)
                return null;

            Init();
            return instance;
        }
    }

    private static object _lock = new object();
    private static bool applicationQuitting = false;

    UI_EventHandler eventHandler = new UI_EventHandler();
    public static UI_EventHandler EventHandler { get { return Instance.eventHandler; } }

    // Popup UI 관리를 위한 정렬 우선도 값과 스택
    int order = 10;
    Stack<UI_Popup> popupStack = new Stack<UI_Popup>();

    public bool IsPopupOpened
    {
        get
        {
            if (popupStack.Count > 0)
                return true;
            else
                return false;
        }
    }

    // UI Object들을 자식으로 가지는 부모 Object
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject("@UI_Root");
            return root;
        }
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        // 일시 정지 중에는 다른 메뉴를 띄우거나 게임이 진행되어서는 안 됨
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (popupStack.Count == 0 && StageManager.Instance.CurrentStage != StageType.Unknown
                && StageManager.Instance.CurrentStage != StageType.Title
                && StageManager.Instance.CurrentStage != StageType.Opening
                && StageManager.Instance.CurrentStage != StageType.Ending
                && StageManager.Instance.CurrentStage != StageType.Loading)
                ShowPopupUI<UI_Pause>();
            else if (!UI_Logo.EscLock)
                ClosePopupUI();

            return;
        }

        if (UI_Dialogue.IsDialogue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                EventHandler.PostNotification(UI_EventHandler.UIEventType.NextDialogue, this);

            return;
        }

        if (!UI_Pause.IsPause && StageManager.Instance.CurrentStage != StageType.Unknown
            && StageManager.Instance.CurrentStage != StageType.Title
            && StageManager.Instance.CurrentStage != StageType.Opening
            && StageManager.Instance.CurrentStage != StageType.Ending
            && StageManager.Instance.CurrentStage != StageType.Loading)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (!UI_CheckUpgrade.IsUpgradeOpen)
                    ShowPopupUI<UI_CheckUpgrade>();
                else
                    ClosePopupUI();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (!UI_AchievementView.IsAchievementOpen)
                    ShowPopupUI<UI_AchievementView>();
                else
                    ClosePopupUI();
            }
        }

        //if (Input.GetKeyDown(KeyCode.L))
        //    ShowPopupUI<UI_Dialogue>();
    }

    private void OnDestroy()
    {
        applicationQuitting = true;
    }

    static void Init()
    {
        lock (_lock)
        {
            if (instance == null)
            {
                GameObject uiManager = GameObject.Find("@UI_Manager");

                if (uiManager == null)
                {
                    uiManager = new GameObject("@UI_Manager");
                    uiManager.AddComponent<UIManager>();
                }

                DontDestroyOnLoad(uiManager);
                instance = uiManager.GetComponent<UIManager>();
            }
        }
    }

    public void SetCanvas(GameObject uiObject, bool isPopup)
    {
        Canvas canvas = Utils.GetAddedComponent<Canvas>(uiObject);
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.overrideSorting = true;

        if (isPopup)
            canvas.sortingOrder = order++;
        else
            canvas.sortingOrder = 0;
    }

    public T ShowSceneUI<T>(string uiName = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(uiName))
            uiName = typeof(T).Name;

        GameObject uiObject = Utils.Instantiate($"UI/Scene/{uiName}");
        T sceneUI = Utils.GetAddedComponent<T>(uiObject);

        uiObject.transform.SetParent(Root.transform);
        return sceneUI;
    }

    public T ShowPopupUI<T>(string uiName = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(uiName))
            uiName = typeof(T).Name;

        GameObject uiObject = Utils.Instantiate($"UI/Popup/{uiName}");
        T popupUI = Utils.GetAddedComponent<T>(uiObject);

        if (!typeof(T).Equals(typeof(UI_SkillCooldown)))
            popupStack.Push(popupUI);

        uiObject.transform.SetParent(Root.transform);
        return popupUI;
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject subItem = Utils.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            subItem.transform.SetParent(parent);

        return Utils.GetAddedComponent<T>(subItem);
    }

    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Utils.Instantiate($"UI/WorldSpace/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        // 시네머신 사용할 경우 Camera 수정해야 함
        Canvas canvas = Utils.GetAddedComponent<Canvas>(go);
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return Utils.GetAddedComponent<T>(go);
    }

    public void ClosePopupUI()
    {
        if (popupStack.Count == 0)
            return;

        UI_Popup popup = popupStack.Pop();
        Utils.Destroy(popup.gameObject);
        popup = null;
        order--;
    }

    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        
        EventHandler.Clear();
    }
}

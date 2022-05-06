using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public static UIManager Instance { get { Init(); return instance; } }

    UI_EventHandler eventHandler = new UI_EventHandler();
    public static UI_EventHandler EventHandler { get { return Instance.eventHandler; } }

    // Popup UI 관리를 위한 정렬 우선도 값과 스택
    int order = 10;
    Stack<UI_Popup> popupStack = new Stack<UI_Popup>();


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
        if (Input.GetKeyDown(KeyCode.A))
        {
            // 무기가 변경될 경우 리스너를 호출
            int weapon = Random.Range(0, 3);
            EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeWeapon, this, weapon);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeHP, this, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeST, this, 0);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 재화 값이 변경될 경우 리스너를 호출
            int goods = Random.Range(0, 10000);
            EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeGoods, this, goods);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!UI_CheckUpgrade.IsOpen)
                ShowPopupUI<UI_CheckUpgrade>();
            else
                ClosePopupUI();

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!UI_SkillCooldown.IsCooldown)
            {
                // 쿨타임 스택을 따로 사용하도록 만듦
                ShowPopupUI<UI_SkillCooldown>();
            }
        }
    }

    static void Init()
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

        // 씬이 시작될 때 이벤트 시스템을 생성해줘야 함
        Object eventSystem = GameObject.FindObjectOfType(typeof(EventSystem));
        if (eventSystem == null)
            Utils.Instantiate("UI/EventSystem").name = "@EventSystem";
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

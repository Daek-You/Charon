using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageType
{
    Unknown,
    Lobby,
    Stage11,
    Stage12,
    Stage13,
    Stage14,
    Stage15,
    Ending
}

public enum MainStageType
{
    Unknown = 0,
    Stage1 = 1,
    Stage2 = 2
}

public class StageManager : MonoBehaviour
{
    private static StageManager instance;
    public static StageManager Instance
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

    // ���� �������� Ÿ���� Ȯ���Ͽ� ���͸� Ȱ��ȭ�� �� ���
    // ���� ȭ�鿡�� �Ͻ����� UI�� ����� �� �ϵ��� �ϴ� �� SceneType�� ���ҵ� ����
    [SerializeField]
    private StageType currentStage = StageType.Unknown;
    public StageType CurrentStage
    {
        get { return currentStage; }
        set
        {
            currentStage = value;
            SetMainStage();
        }
    }

    [SerializeField]
    private MainStageType currentMainStage = MainStageType.Unknown;
    public MainStageType CurrentMainStage { get { return currentMainStage; } }

    // Ŭ���� ���θ� �Ǵ��ϱ� ���� ���� �ý��� Ȱ��
    // Ŭ���� ���ΰ� ��ȭ�� ��� �˸��� ����, UI�� ǥ���ϰų� �������� �庮�� �����ϴ� ���� ��ҷ� Ȱ��
    [SerializeField]
    private bool isCleared = false;
    public bool IsCleared
    {
        get { return isCleared; }
        set
        {
            bool postValue = isCleared;
            if (postValue != value)
            {
                isCleared = value;
                UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeClear, this, isCleared);
            }
        }
    }

    [SerializeField]
    private int clearCount = 0;
    [SerializeField]
    private int currentCount = 0;
    public int CurrentCount
    {
        get { return currentCount; }
        set
        {
            currentCount = value;
            if (currentCount >= clearCount)
                IsCleared = true;
        }
    }

    [SerializeField]
    private bool isClearedByLoad = false;
    public bool IsClearedByLoad { get { return isClearedByLoad; } set { isClearedByLoad = value; } }

    // Enemy Object���� �ڽ����� ������ �θ� Object
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@Enemy_Root");
            if (root == null)
                root = new GameObject("@Enemy_Root");
            return root;
        }
    }

    private void Start()
    {
        Init();
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.DieEnemy, OnCheckCount);
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
                GameObject stageManager = GameObject.Find("@Stage_Manager");
                if (stageManager == null)
                {
                    stageManager = new GameObject { name = "@Stage_Manager" };
                    stageManager.AddComponent<StageManager>();
                }

                DontDestroyOnLoad(stageManager);
                instance = stageManager.GetComponent<StageManager>();
            }
        }
    }

    public void SetStage()
    {
        if (currentStage == StageType.Unknown || currentStage == StageType.Lobby || currentStage == StageType.Ending)
            return;

        SetEnemies();
        ActiveStage(currentStage);
    }

    public void ActiveStage(StageType type)
    {
        if (currentStage == StageType.Ending)
            return;

        CurrentStage = type;
        isCleared = false;
        clearCount = 0;
        currentCount = 0;

        if (isClearedByLoad)
        {
            IsCleared = true;
            isClearedByLoad = false;
            return;
        }

        ActiveEnemies();
    }

    public void SetMainStage()
    {
        if (currentStage == StageType.Unknown || currentStage == StageType.Lobby || currentStage == StageType.Ending)
            return;

        string stage = currentStage.ToString();
        stage = stage.Substring(stage.Length - 2, 1);
        int stageInt = int.Parse(stage);
        currentMainStage = (MainStageType)stageInt;
    }

    private void SetEnemies()
    {
        // ���� ���������� �´� Dictionary�� ���͸� ����
        // �̸� ������ �� �ֵ��� spawnDictionary�� ����
        List<EnemyData> enemyData;
        DataManager.EnemyDict.TryGetValue(currentMainStage, out enemyData);

        foreach (EnemyData enemy in enemyData)
        {
            GameObject root = Utils.FindChild(Root, enemy.sstage);
            if (root == null)
                root = new GameObject(enemy.sstage);
            root.transform.SetParent(Root.transform);

            GameObject go = Utils.Instantiate($"Units/{enemy.type}");
            go.transform.position = new Vector3(enemy.position[0], enemy.position[1], enemy.position[2]);
            go.SetActive(false);

            go.transform.SetParent(root.transform);
        }
    }

    private void ActiveEnemies()
    {
        if (currentStage == StageType.Unknown || currentStage == StageType.Lobby || currentStage == StageType.Ending)
            return;

        // ���� ���������� �´� spawnDictionary�� ���͸� Ȱ��ȭ
        GameObject root = Utils.FindChild(Root, currentStage.ToString());
        if (root == null)
            return;

        for (int i = 0; i < root.transform.childCount; i++)
        {
            root.transform.GetChild(i).gameObject.SetActive(true);
            clearCount++;
        }
    }

    public void OnCheckCount(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        // Ư�� ���Ϳ� ����ġ�� �ְų�, ������ �Ǻ��ϴ� �뵵�� param ��� ����
        CurrentCount++;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            CurrentCount = clearCount;
    }
}

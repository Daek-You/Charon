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
        }
    }

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

    private int clearCount = 3;
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

    static Dictionary<StageType, EnemyData[]> enemyDictionary = new Dictionary<StageType, EnemyData[]>();
    private Dictionary<StageType, Enemy[]> spawnDictionary = new Dictionary<StageType, Enemy[]>();

    private void Start()
    {
        Init();
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

                LoadEnemyData();
            }
        }
    }

    public void SetStage()
    {
        if (currentStage == StageType.Unknown && currentStage == StageType.Lobby)
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
        currentCount = 0;

        if (isClearedByLoad)
        {
            IsCleared = true;
            isClearedByLoad = false;
            return;
        }

        ActiveEnemies();
    }

    private static void LoadEnemyData()
    {
        // Json ���Ϸκ��� �����͸� �ҷ��� Dictionary�� ����
    }

    private void SetEnemies()
    {
        // ���� ���������� �´� Dictionary�� ���͸� ����
        // �̸� ������ �� �ֵ��� spawnDictionary�� ����
    }

    private void ActiveEnemies()
    {
        // ���� ���������� �´� spawnDictionary�� ���͸� Ȱ��ȭ
        // Ȱ��ȭ �� ��ġ = ClearCount
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            CurrentCount = clearCount;
    }
}

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

    // 현재 스테이지 타입을 확인하여 몬스터를 활성화할 때 사용
    // 메인 화면에서 일시정지 UI를 띄우지 못 하도록 하는 등 SceneType의 역할도 겸함
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

    // 클리어 여부를 판단하기 위해 업적 시스템 활용
    // 클리어 여부가 변화할 경우 알림을 보내, UI를 표시하거나 스테이지 장벽을 제거하는 등의 요소로 활용
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
        // Json 파일로부터 데이터를 불러와 Dictionary에 저장
    }

    private void SetEnemies()
    {
        // 메인 스테이지에 맞는 Dictionary의 몬스터를 생성
        // 이를 관리할 수 있도록 spawnDictionary에 저장
    }

    private void ActiveEnemies()
    {
        // 현재 스테이지에 맞는 spawnDictionary의 몬스터를 활성화
        // 활성화 한 수치 = ClearCount
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            CurrentCount = clearCount;
    }
}

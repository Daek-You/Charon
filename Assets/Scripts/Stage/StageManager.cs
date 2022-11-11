using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageType
{
    Unknown,
    Title,
    Opening,
    Loading,
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
    public static StageManager Instance { get { Init(); return instance; } }

    QuestGiver giver;

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

    [SerializeField]
    private bool isClearedByLoad = false;
    public bool IsClearedByLoad { get { return isClearedByLoad; } set { isClearedByLoad = value; } }

    static Dictionary<StageType, EnemyData[]> enemyDictionary = new Dictionary<StageType, EnemyData[]>();
    static Dictionary<StageType, Quest> questDictionary = new Dictionary<StageType, Quest>();

    private void Start()
    {
        Init();
    }

    static void Init()
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
            LoadQuestData();
        }
    }

    public void SetStage()
    {
        if (currentStage == StageType.Unknown && currentStage == StageType.Lobby)
            return;
        if (giver == null)
            giver = Utils.GetAddedComponent<QuestGiver>(instance.gameObject);

        // 스테이지 씬에 맞는 몬스터들을 배치 후 비활성화

        ActiveStage(currentStage);
    }

    public void ActiveStage(StageType type)
    {
        if (currentStage == StageType.Ending)
            return;

        CurrentStage = type;
        isCleared = false;

        // 현재 스테이지에 맞는 몬스터를 활성화
        Debug.Log($"{currentStage} Start!");

        giver.Register(questDictionary[currentStage]);
        QuestManager.Instance.onQuestCompleted += OnClearStage;

        if (isClearedByLoad)
        {
            giver.Complete();
            // 현재 스테이지의 몬스터들을 비활성화
            Debug.Log($"{currentStage} Loaded!");
        }
    }

    private void OnClearStage(Quest quest)
    {
        Quest stageQuest;
        questDictionary.TryGetValue(CurrentStage, out stageQuest);

        if (stageQuest.CodeName == quest.CodeName)
        {
            IsCleared = true;
            Debug.Log($"{currentStage} Clear!");
        }
    }

    /* 여기 수정해줘 수용이 */
    private static void LoadEnemyData()
    {
        EnemyData[] temp = new EnemyData[2];

        Vector3[] firstPosition = new Vector3[1];
        firstPosition[0] = Vector3.zero;
        Vector3[] secondPosition = new Vector3[2];
        secondPosition[0] = new Vector3(1, 0, 1);
        secondPosition[1] = new Vector3(-1, 0, -1);

        //temp[0] = new EnemyData(MonsterType.private_k, 1, firstPosition);
        //temp[1] = new EnemyData(MonsterType.private_a, 2, secondPosition);

        enemyDictionary.Clear();
        enemyDictionary.Add(StageType.Stage11, temp);
        enemyDictionary.Add(StageType.Stage12, temp);
    }

    private static void LoadQuestData()
    {
        Quest quest = Resources.Load<Quest>("Contents/Achievement/Quest_Stage11");
        questDictionary.Add(StageType.Stage11, quest);
    }
}

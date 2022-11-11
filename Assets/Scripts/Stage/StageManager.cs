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

        // �������� ���� �´� ���͵��� ��ġ �� ��Ȱ��ȭ

        ActiveStage(currentStage);
    }

    public void ActiveStage(StageType type)
    {
        if (currentStage == StageType.Ending)
            return;

        CurrentStage = type;
        isCleared = false;

        // ���� ���������� �´� ���͸� Ȱ��ȭ
        Debug.Log($"{currentStage} Start!");

        giver.Register(questDictionary[currentStage]);
        QuestManager.Instance.onQuestCompleted += OnClearStage;

        if (isClearedByLoad)
        {
            giver.Complete();
            // ���� ���������� ���͵��� ��Ȱ��ȭ
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

    /* ���� �������� ������ */
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

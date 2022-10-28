using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager Instance { get { Init(); return _instance; } }

    private GameData _saveData;
    public GameData SaveData
    {
        get
        {
            if (_saveData == null)
            {
                _saveData = new GameData();
                Debug.Log("Create New Data.");
            }
            return _saveData;
        }
    }

    private OptionData _optData;
    public OptionData OptData
    {
        get
        {
            if (_optData == null)
            {
                LoadOptionData();
            }
            return _optData;
        }
    }

    private int _dataIndex;
    public int DataIndex { get { return _dataIndex; } set { _dataIndex = value; } }

    public void Start()
    {
        Init();
    }

    public static void Init()
    {
        if (_instance == null)
        {
            GameObject dataManager = GameObject.Find("@Data_Manager");
            if (dataManager == null)
            {
                dataManager = new GameObject { name = "@Data_Manager" };
                dataManager.AddComponent<DataManager>();
            }

            DontDestroyOnLoad(dataManager);
            _instance = dataManager.GetComponent<DataManager>();
        }
    }

    // Data Manager�� Index�� ������ �ִµ�, ���ڷ� ���� �ʿ䰡 �ֳ�?
    public void SaveGameData(int index)
    {
        SetSaveData();

        string fileName = $"/CharonData{index}.json";
        string filePath = Application.persistentDataPath + fileName;
        string ToJsonData = JsonUtility.ToJson(SaveData);
        File.WriteAllText(filePath, ToJsonData);
        Debug.Log($"Success to Save Data.");
    }

    public GameData LoadGameData(int index)
    {
        string fileName = $"/CharonData{index}.json";
        string filePath = Application.persistentDataPath + fileName;

        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            _saveData = JsonUtility.FromJson<GameData>(FromJsonData);
            Debug.Log($"Success to Load Data.");
        }
        else
        {
            _saveData = new GameData();
            Debug.Log("Create New Data.");
        }

        return SaveData;
    }

    // Save, Load Game Data�� ������ ����
    // json ��ũ��Ʈ �۾� �� ���� �������� Ȯ���� �� ��ü������ ������ �ʿ�
    public void SaveOptionData()
    {
        string fileName = $"/CharonOptionData.json";
        string filePath = Application.persistentDataPath + fileName;
        string ToJsonData = JsonUtility.ToJson(OptData);
        File.WriteAllText(filePath, ToJsonData);
        Debug.Log($"Success to Save Option.");
    }

    public OptionData LoadOptionData()
    {
        string fileName = $"/CharonOptionData.json";
        string filePath = Application.persistentDataPath + fileName;

        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            _optData = JsonUtility.FromJson<OptionData>(FromJsonData);
            Debug.Log($"Success to Load Option.");
        }
        else
        {
            _optData = new OptionData();
            Debug.Log("Create New Option Data.");
        }

        return OptData;
    }

    public void SetSaveData()
    {
        if (!SaveData.IsSaved)
            _saveData.IsSaved = true;

        // �÷��̾� ���� �� ���� ����

        _saveData.CurrentPosition = GameObject.Find("Sondol").transform.position;

        _saveData.CurrentStage = StageManager.Instance.CurrentStage;
        _saveData.IsCleared = StageManager.Instance.IsCleared;
    }

    public void StartGameData()
    {
        _saveData = null;
    }
}
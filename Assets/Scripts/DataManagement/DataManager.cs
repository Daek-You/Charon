using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (applicationQuitting)
                return null;

            Init();
            return _instance;
        }
    }

    private static object _lock = new object();
    private static bool applicationQuitting = false;

    private GameData _saveData;
    public GameData SaveData
    {
        get
        {
            if (_saveData == null)
            {
                _saveData = new GameData();
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

    public static Dictionary<MainStageType, List<EnemyData>> EnemyDict { get; private set; } = new Dictionary<MainStageType, List<EnemyData>>();

    public void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        applicationQuitting = true;
    }

    public static void Init()
    {
        lock (_lock)
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

                EnemyDict = LoadJson<EnemyDataForLoad, MainStageType, List<EnemyData>>("EnemyData").MakeDict();
            }
        }
    }

    // Data Manager가 Index를 가지고 있는데, 인자로 받을 필요가 있나?
    public void SaveGameData(int index, bool saveValue = true)
    {
        if (!SaveData.IsSaved && saveValue)
            _saveData.IsSaved = true;
        SetSaveData();

        string fileName = $"/CharonData{index}.json";
        string filePath = Application.persistentDataPath + fileName;
        string ToJsonData = JsonUtility.ToJson(SaveData);
        File.WriteAllText(filePath, ToJsonData);
    }

    public GameData LoadGameData(int index)
    {
        string fileName = $"/CharonData{index}.json";
        string filePath = Application.persistentDataPath + fileName;

        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            _saveData = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            _saveData = new GameData();
        }

        return SaveData;
    }

    // Save, Load Game Data와 유사한 구조
    // json 스크립트 작업 시 통합 가능한지 확인한 후 전체적으로 수정이 필요
    public void SaveOptionData()
    {
        string fileName = $"/CharonOptionData.json";
        string filePath = Application.persistentDataPath + fileName;
        string ToJsonData = JsonUtility.ToJson(OptData);
        File.WriteAllText(filePath, ToJsonData);
    }

    public OptionData LoadOptionData()
    {
        string fileName = $"/CharonOptionData.json";
        string filePath = Application.persistentDataPath + fileName;

        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            _optData = JsonUtility.FromJson<OptionData>(FromJsonData);
        }
        else
        {
            _optData = new OptionData();
        }

        return OptData;
    }

    public void SetSaveData()
    {
        _saveData.CurrentPosition = GameObject.Find("Sondol").transform.position;

        _saveData.CurrentHP = Player.Instance.CurrentHP;
        _saveData.CurrentST = Player.Instance.weaponManager.Weapon.CurrentSkillGauge;
        _saveData.WeaponName = Player.Instance.weaponManager.GetWeaponName();

        _saveData.CurrentStage = StageManager.Instance.CurrentStage;
        _saveData.IsCleared = StageManager.Instance.IsCleared;
    }

    public void StartGameData()
    {
        _saveData = null;
    }

    static Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Utils.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}

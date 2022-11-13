using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    //�̱������� ����.
    private static StatManager instance = null;
    public static StatManager Instance { get { return instance; } }

    private Stat maxHP;            //�ִ�ü��
    private Stat moveSpeed;        //�̵��ӵ�
    private Stat attackDamage;     //���ܷ�. ������ ������ ���� �޶���.
    private Stat armor;            //����
    private Stat dashCount;        //�뽬 Ƚ��
    
    //�ٸ� ��ũ��Ʈ���� ������ ���� ����
    public float currentMaxHP { get { return maxHP.currentValue; } }
    public float currentMoveSpeed { get { return moveSpeed.currentValue; } }
    public float currentAttackDamage { get { return attackDamage.currentValue; } }
    public float currentArmor { get { return armor.currentValue; } }
    public int currentDashCount { get { return (int)dashCount.currentValue; } }

    public int CurHPLevel { get { return maxHP.currentReinforceLevel; } }
    public int CurArmorLevel { get { return armor.currentReinforceLevel; } }
    public int CurSpeedLevel { get { return moveSpeed.currentReinforceLevel; } }
    public int CurDashLevel { get { return dashCount.currentReinforceLevel; } }

    private int gold;
    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeGoods, this, gold);
        }
    }

    QuestReporter[] reporters;

    //�̱��� ���� ����.
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        DestroyImmediate(this.gameObject);
    }

    void Start()
    {
        InitStatus();
        reporters = GetComponents<QuestReporter>();
    }
    private void Update()
    {
        //���Ⱑ �ٲ���� �� ��� �ִ� ���⿡ ���� attack damage�� �ҷ����� �Լ� �ʿ�.
        //Player.Instance.weaponManager.Weapon.Name �̷������� �޾ƿ��� ��.
    }

    //�ʱ� ���� ����.
    public void InitStatus()
    {
        maxHP = new Stat(StatType.MAX_HP, initValue: 1000, currentReinforceLevel: 0, maxReinforceLevel: 10);
        moveSpeed = new Stat(StatType.MOVE_SPEED, initValue: 3, currentReinforceLevel: 0, maxReinforceLevel: 10);
        dashCount = new Stat(StatType.DASH_COUNT, initValue: 1,  currentReinforceLevel: 0, maxReinforceLevel: 1);
        //attackDamage = new Stat(StatType.ATTACK_DAMAGE, initValue: 100, increasingAmount: 20, currentReinforceLevel: 0, maxReinforceLevel: 10);
        armor = new Stat(StatType.ARMOR, initValue: 50, currentReinforceLevel: 0, maxReinforceLevel: 10);

        SetAllStatus();
    }

    //��ȭ�� ������ ���� ���ȿ� �����Ű�� �Լ�.
    public void SetStatus(StatType statType)
    {
        switch (statType)
        {
            case StatType.MAX_HP:
                maxHP.currentValue = maxHP.initValue + DataManager.MaxHpDict[maxHP.currentReinforceLevel].increasingAmount;
                return;
            case StatType.MOVE_SPEED:
                moveSpeed.currentValue = moveSpeed.initValue + DataManager.MoveSpeedDict[moveSpeed.currentReinforceLevel].increasingAmount;
                return;
            case StatType.DASH_COUNT:
                dashCount.currentValue = dashCount.initValue + DataManager.DashCountDict[dashCount.currentReinforceLevel].increasingAmount;
                return;
            case StatType.ARMOR:
                armor.currentValue = armor.initValue + DataManager.ArmorDict[armor.currentReinforceLevel].increasingAmount;
                return;
            default:
                Player.Instance.OnUpdateStat(currentMaxHP, currentMaxHP, currentArmor, currentMoveSpeed, currentDashCount); 
                return;
        }
    }

    public void UpgradeStatus(StatType statType)
    {
        //��Ŀ�� ���� ��� �̷��� ���Կ�
        

        switch (statType)
        {
            case StatType.MAX_HP:
                if (maxHP.currentReinforceLevel < maxHP.maxReinforceLevel)
                {
                    maxHP.currentReinforceLevel++;
                    UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeUpgrade, this, statType);
                    reporters[0].Report();
                    SetStatus(statType);
                }                
                return;
            case StatType.ARMOR:
                if (armor.currentReinforceLevel < armor.maxReinforceLevel)
                {
                    armor.currentReinforceLevel++;
                    UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeUpgrade, this, statType);
                    reporters[2].Report();
                    SetStatus(statType);
                }
                return;
            case StatType.MOVE_SPEED:
                if (moveSpeed.currentReinforceLevel < moveSpeed.maxReinforceLevel)
                {
                    moveSpeed.currentReinforceLevel++;
                    UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeUpgrade, this, statType);
                    reporters[3].Report();
                    SetStatus(statType);
                }
                return;
            case StatType.DASH_COUNT:
                if (dashCount.currentReinforceLevel < dashCount.maxReinforceLevel)
                {
                    dashCount.currentReinforceLevel++;
                    UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeUpgrade, this, statType);
                    reporters[1].Report();
                    SetStatus(statType);
                }
                return;
            case StatType.PADDLE_DAMAGE:
                WeaponUpgradeFunction();
                return;
            //case StatType.SICKLE_DAMAGE:
            //    if (dashCount.currentReinforceLevel < dashCount.maxReinforceLevel)
            //        dashCount.currentReinforceLevel++;
            //    SetStatus(statType);
            //    return;
            //case StatType.GOURD_DAMAGE:
            //    if (dashCount.currentReinforceLevel < dashCount.maxReinforceLevel)
            //        dashCount.currentReinforceLevel++;
            //    SetStatus(statType);
        }
    }

    void WeaponUpgradeFunction()
    {
        float CurrentWeaponDamage = Player.Instance.weaponManager.Weapon.AttackDamage;
        string CurrentWeaponName = Player.Instance.weaponManager.Weapon.Name;
        float CurrentWeaponAttackSpeed = Player.Instance.weaponManager.Weapon.AttackSpeed;
        float CurrentWeaponAttackRange = Player.Instance.weaponManager.Weapon.AttackRange;

        Player.Instance.weaponManager.Weapon.SetWeaponData(CurrentWeaponName, CurrentWeaponDamage, CurrentWeaponAttackSpeed, CurrentWeaponAttackRange);
    }

    public void SetReinforceLevel()
    {
        maxHP.currentReinforceLevel = 0;
        armor.currentReinforceLevel = 0;
        moveSpeed.currentReinforceLevel = 0;
        dashCount.currentReinforceLevel = 0;

        SetAllStatus();
    }

    public void SetReinforceLevel(GameData data)
    {
        if (data == null)
        {
            SetReinforceLevel();
            return;
        }

        maxHP.currentReinforceLevel = data.CurrentHPReinforceLevel;
        armor.currentReinforceLevel = data.CurrentArmorReinforceLevel;
        moveSpeed.currentReinforceLevel = data.CurrentMoveSpeedReinforceLevel;
        dashCount.currentReinforceLevel = data.CurrentDashCountReinforceLevel;

        SetAllStatus();
    }

    public void SetAllStatus()
    {
        SetStatus(StatType.MAX_HP);
        SetStatus(StatType.ARMOR);
        SetStatus(StatType.MOVE_SPEED);
        SetStatus(StatType.DASH_COUNT);
    }

    public List<StatType> GetStatList()
    {
        List<StatType> list = new List<StatType>();
        // ���� ���� �������� �߰��ؾ� ��
        list.Add(StatType.PADDLE_DAMAGE);
        list.Add(StatType.MAX_HP);
        list.Add(StatType.ARMOR);
        list.Add(StatType.MOVE_SPEED);
        list.Add(StatType.DASH_COUNT);

        return list;
    }
}

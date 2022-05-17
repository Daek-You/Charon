using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get { return instance; } }
    /// <summary>
    /// ��ȭ ���� ��ġ�� ����� �ִ� ü���� ��ȯ�մϴ�.
    /// </summary>
    public int MaxHP { get { return CalculateStat(StatType.MAX_HP); } }

    /// <summary>
    /// ��ȭ ���� ��ġ�� ����� �̵� �ӵ��� ��ȯ�մϴ�.
    /// </summary>
    public int MoveSpeed { get { return CalculateStat(StatType.MOVE_SPEED); } }

    /// <summary>
    /// ��ȭ ���� ��ġ�� ����� ���ݷ��� ��ȯ�մϴ�.
    /// </summary>
    public int AttackDamage { get { return CalculateStat(StatType.ATTACK_DAMAGE); } }

    /// <summary>
    /// ��ȭ ���� ��ġ�� ����� ������ ��ȯ�մϴ�.
    /// </summary>
    public int Armor { get { return CalculateStat(StatType.ARMOR); } }

    /// <summary>
    /// ��ȭ ���� ��ġ�� ����� ���� ��� ������ �뽬 Ƚ���� ��ȯ�մϴ�.
    /// </summary>
    public int DashCount { get { return CalculateStat(StatType.DASH_COUNT); } }

    /// <summary>
    /// ���� ������ �ִ� ��ȭ�� ��ȯ�մϴ�.
    /// </summary>
    public int Gold { get { return gold; } }

    private static StatManager instance = null;
    private Dictionary<StatType, Stat> stats = new Dictionary<StatType, Stat>();
    private Stat maxHP;
    private Stat moveSpeed;
    private Stat attackDamage;
    private Stat armor;
    private Stat dashCount;
    private int gold;



    void Awake()
    {
        if(instance == null)
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
    }


    /// <summary>
    /// ���� ��ȭ�� �߰��մϴ�.
    /// </summary>
    /// <param name="gold">�߰��� �ݾ�</param>
    public void AddGold(int amount)
    {
        if (amount > 0)
        {
            if (int.MaxValue - gold > amount)
            {
                gold += amount;
            }

            else if(int.MaxValue - gold == amount)
                gold = int.MaxValue;

            // [������ ����] ��ȭ ���� �̺�Ʈ �߻�
        }
    }


    /// <summary>
    /// ���� ��ȭ���� ������ �ݾ��� �����մϴ�.
    /// </summary>
    /// <param name="amount">������ �ݾ�</param>
    public void PayGold(int amount)
    {
        if (amount > 0 && gold >= amount)
        {
            gold -= amount;
            // [������ ����] ��ȭ ���� �̺�Ʈ �߻�
        }
    }


    /// <summary>
    /// ��ȭ �ý��ۿ��� ȣ���� ���, ������ ������ ���� �����մϴ�.
    /// </summary>
    /// <param name="requester">�ܺο��� ȣ���ϴ� Ŭ����</param>
    /// <param name="stat">������ ����</param>
    /// <param name="newState">���� ������ ��</param>
    public void SetStatus(Component requester, StatType stat, Stat newState)
    {
        //if(requester is ReinforceSystem)   ���� ���� ������ �� �� �ִ� �ܺδ� ���� ��ȭ �ý��ۻ�
        //{
            switch (stat)
            {
                case StatType.MAX_HP:
                    maxHP = newState;
                    break;
                case StatType.MOVE_SPEED:
                    moveSpeed = newState;
                    break;
                case StatType.ATTACK_DAMAGE:
                    attackDamage = newState;
                    break;
                case StatType.DASH_COUNT:
                    dashCount = newState;
                    break;
                case StatType.ARMOR:
                    armor = newState;
                    break;
            }
        //}
      

        // [������ ����] ���� ���� �̺�Ʈ �߻�
    }


    /// <summary>
    /// ���� ������ �ִ� ���� �������� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public Dictionary<StatType, Stat> GetStats()
    {
        stats.Clear();
        stats.Add(StatType.MAX_HP, maxHP);
        stats.Add(StatType.ARMOR, armor);
        stats.Add(StatType.MOVE_SPEED, moveSpeed);
        stats.Add(StatType.ATTACK_DAMAGE, attackDamage);
        stats.Add(StatType.DASH_COUNT, dashCount);
        return stats;
    }


    private void InitStatus()
    {

        /// �ϴ� ���� �ý��� ����� ������ �⺻ ��������
        maxHP = new Stat(initValue: 1000, IncreasingAmout: 200, currentReinforceLevel: 0, maxReinforceLevel: 10);
        moveSpeed = new Stat(initValue: 300, IncreasingAmout: 20, currentReinforceLevel: 0, maxReinforceLevel: 10);
        dashCount = new Stat(initValue: 1, IncreasingAmout: 1, currentReinforceLevel: 0, maxReinforceLevel: 1);
        attackDamage = new Stat(initValue: 150, IncreasingAmout: 0.2f, currentReinforceLevel: 0, maxReinforceLevel: 10);
        armor = new Stat(initValue: 50, IncreasingAmout: 0.2f, currentReinforceLevel: 0, maxReinforceLevel: 10);
        gold = 100;
    }

    private int CalculateStat(StatType stat)
    {
        int result = 0;
   
        switch (stat)
        {
            /// ������ ���ȵ� ��� ���  :  �ʱⰪ + ������ * ���� ��ȭ ����
            case StatType.MAX_HP:
                result = (int)(maxHP.initValue + maxHP.IncreasingAmount * maxHP.currentReinforceLevel);
                break;
            case StatType.MOVE_SPEED:
                result = (int)(moveSpeed.initValue + moveSpeed.IncreasingAmount * moveSpeed.currentReinforceLevel);
                break;
            case StatType.DASH_COUNT:
                result = (int)(dashCount.initValue + dashCount.IncreasingAmount * dashCount.currentReinforceLevel);
                break;


            /// # ������ ���ȵ� ��� ���  :  �ʱⰪ * (1 + ������ * ���� ��ȭ ����)
            case StatType.ATTACK_DAMAGE:
                result = (int)(attackDamage.initValue * (1 + attackDamage.IncreasingAmount * attackDamage.currentReinforceLevel));
                break;
            case StatType.ARMOR:
                result = (int)(armor.initValue * (1 + armor.IncreasingAmount * armor.currentReinforceLevel));
                break;
        }

        return result;
    }
}

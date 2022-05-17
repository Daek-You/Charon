using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get { return instance; } }
    /// <summary>
    /// 강화 레벨 수치가 적용된 최대 체력을 반환합니다.
    /// </summary>
    public int MaxHP { get { return CalculateStat(StatType.MAX_HP); } }

    /// <summary>
    /// 강화 레벨 수치가 적용된 이동 속도를 반환합니다.
    /// </summary>
    public int MoveSpeed { get { return CalculateStat(StatType.MOVE_SPEED); } }

    /// <summary>
    /// 강화 레벨 수치가 적용된 공격력을 반환합니다.
    /// </summary>
    public int AttackDamage { get { return CalculateStat(StatType.ATTACK_DAMAGE); } }

    /// <summary>
    /// 강화 레벨 수치가 적용된 방어력을 반환합니다.
    /// </summary>
    public int Armor { get { return CalculateStat(StatType.ARMOR); } }

    /// <summary>
    /// 강화 레벨 수치가 적용된 연속 사용 가능한 대쉬 횟수를 반환합니다.
    /// </summary>
    public int DashCount { get { return CalculateStat(StatType.DASH_COUNT); } }

    /// <summary>
    /// 현재 가지고 있는 재화를 반환합니다.
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
    /// 기존 재화에 추가합니다.
    /// </summary>
    /// <param name="gold">추가할 금액</param>
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

            // [수용이 공간] 재화 변경 이벤트 발생
        }
    }


    /// <summary>
    /// 기존 재화에서 지불할 금액을 차감합니다.
    /// </summary>
    /// <param name="amount">지불할 금액</param>
    public void PayGold(int amount)
    {
        if (amount > 0 && gold >= amount)
        {
            gold -= amount;
            // [수용이 공간] 재화 변경 이벤트 발생
        }
    }


    /// <summary>
    /// 강화 시스템에서 호출할 경우, 지정한 스탯의 값을 변경합니다.
    /// </summary>
    /// <param name="requester">외부에서 호출하는 클래스</param>
    /// <param name="stat">수정할 스탯</param>
    /// <param name="newState">새로 수정할 값</param>
    public void SetStatus(Component requester, StatType stat, Stat newState)
    {
        //if(requester is ReinforceSystem)   스탯 원본 수정을 할 수 있는 외부는 오직 강화 시스템뿐
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
      

        // [수용이 공간] 스탯 변경 이벤트 발생
    }


    /// <summary>
    /// 현재 가지고 있는 스탯 정보들을 반환합니다.
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

        /// 일단 저장 시스템 만들기 전까지 기본 스탯으로
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
            /// 합적용 스탯들 계산 결과  :  초기값 + 증가량 * 현재 강화 레벨
            case StatType.MAX_HP:
                result = (int)(maxHP.initValue + maxHP.IncreasingAmount * maxHP.currentReinforceLevel);
                break;
            case StatType.MOVE_SPEED:
                result = (int)(moveSpeed.initValue + moveSpeed.IncreasingAmount * moveSpeed.currentReinforceLevel);
                break;
            case StatType.DASH_COUNT:
                result = (int)(dashCount.initValue + dashCount.IncreasingAmount * dashCount.currentReinforceLevel);
                break;


            /// # 곱적용 스탯들 계산 결과  :  초기값 * (1 + 증가량 * 현재 강화 레벨)
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

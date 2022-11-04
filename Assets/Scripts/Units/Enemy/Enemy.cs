using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CharacterController;

public abstract class Enemy : MonoBehaviour, IHittable
{
    public string Name { get { return _name; } }
    public float MaxHP { get { return maxHP; } }
    public float CurrentHP { get { return currentHP; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float Armor { get { return armor; } }
    public float AttackDamage { get { return attackDamage; } }
    public float RotationSpeed { get { return rotationSpeed; } }
    public Transform Target { get { return target; } }
    public float AttackDelay { get { return attackDelay; } }


    #region #몬스터 스탯
    [Header("몬스터 스탯")]
    [SerializeField] protected string _name;
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float armor;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackDelay;

    [Header("옵션")]
    [SerializeField] protected Transform target;
    [SerializeField] protected float rotationSpeed;   // 15f가 적당
    #endregion

    public NavMeshAgent agent { get; private set; }
    public Animator animator { get; private set; }
    public Rigidbody rigidBody { get; private set; }
    public StateMachine stateMachine { get; private set; }

    private Coroutine attackDelayCoroutine;
    public bool isAlived { get; private set; }
    public bool isMoving { get; private set; }

    #region# Unity Functions
    void Update()
    {
        CalculateAliveOrMoving();
        stateMachine?.UpdateState();
    }

    void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }
    #endregion


    protected void InitSettings()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        agent.updateRotation = false;
        agent.isStopped = true;

        stateMachine = new StateMachine(StateName.ENEMY_MOVE, new EnemyMoveState(this));
        stateMachine.AddState(StateName.ENEMY_ATTACK, new EnemyAttackState(this));
    }

    /// <summary>
    /// Rigidbody의 isKinematic, NavMeshAgent를 활성화/비활성화합니다.
    /// </summary>
    /// <param name="enable">활성화/비활성화 여부</param>
    public void SetRigidAndNavMeshAgent(bool enable)
    {
        agent.enabled = enable;
        rigidBody.isKinematic = enable;
    }

    public void SetStats(float maxHP, float currentHP, float moveSpeed, float armor, float attackDamage)
    {
        this.maxHP = maxHP;
        this.currentHP = currentHP;
        this.moveSpeed = moveSpeed;
        this.armor = armor;
        this.attackDamage = attackDamage;
    }

    public void Damaged(float damage)
    {
        currentHP = Mathf.Clamp(currentHP - damage, 0, maxHP);

        if(!Mathf.Approximately(currentHP, 0f))
        {
            ///죽음 상태 전환
            ///return;
        }
        // 피격 상태 전환

    }

    protected void CalculateAliveOrMoving()
    {
        if (agent.enabled)
        {
            isAlived = agent.velocity.sqrMagnitude >= 0.1f * 0.1f && agent.remainingDistance <= agent.stoppingDistance;
            isMoving = agent.desiredVelocity.sqrMagnitude >= 0.1f * 0.1f;
        }
    }

    public void CheckAttackDelay()
    {
        if (attackDelayCoroutine != null)
            StopCoroutine(attackDelayCoroutine);
        attackDelayCoroutine = StartCoroutine(AttackDelayCoroutine());
    }

    private IEnumerator AttackDelayCoroutine()
    {
        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer >= attackDelay)
            {
                EnemyAttackState attackState = stateMachine.GetState(StateName.ENEMY_ATTACK) as EnemyAttackState;
                attackState.isAttack = false;
                break;
            }

            yield return null;
        }
    }
}

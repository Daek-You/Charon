using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CharacterController;
using Unity.XR.Oculus.Input;

public abstract class Enemy : MonoBehaviour, IHittable
{
    public string Name { get { return _name; } }
    public float MaxHP { get { return maxHP; } }
    public float CurrentHP { get { return currentHP; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float Armor { get { return armor; } }
    public EnemyWeapon Weapon { get { return weapon; } }
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
    [SerializeField] protected EnemyWeapon weapon;
    [SerializeField] protected float attackDelay;

    [Header("옵션")]
    [SerializeField] protected Transform target;
    [SerializeField] protected float rotationSpeed;   // 15f가 적당
    #endregion

    public const float HIT_TIME = 0.75f;
    public NavMeshAgent agent { get; private set; }
    public Animator animator { get; private set; }
    public Rigidbody rigidBody { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public AudioSource audioSource { get; private set; }
    public AudioClip hitSound;

    public SkinnedMeshRenderer skinnedMeshRenderer { get; private set; }
    public Material originMaterial { get; private set; }


    private Coroutine attackDelayCoroutine;
    private Coroutine hitDelayCoroutine;
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
        audioSource = GetComponent<AudioSource>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originMaterial = skinnedMeshRenderer.material;

        agent.updateRotation = false;
        agent.isStopped = true;

        stateMachine = new StateMachine(StateName.ENEMY_MOVE, new EnemyMoveState(this));
        stateMachine.AddState(StateName.ENEMY_ATTACK, new EnemyAttackState(this));
        stateMachine.AddState(StateName.ENEMY_HIT, new EnemyHitState(this));
        stateMachine.AddState(StateName.ENEMY_DIE, new EnemyDieState(this));
    }


    public void SetStats(float maxHP, float currentHP, float moveSpeed, float armor)
    {
        this.maxHP = maxHP;
        this.currentHP = currentHP;
        this.moveSpeed = moveSpeed;
        this.armor = armor;
    }

    public void SetWeapon(EnemyWeapon otherWeapon)
    {
        this.weapon = otherWeapon;
    }

    public void Damaged(float damage)
    {
        currentHP = Mathf.Clamp(currentHP - (damage - armor * 0.01f), 0, maxHP);

        if (Mathf.Approximately(currentHP, 0f))
        {
            stateMachine.ChangeState(StateName.ENEMY_DIE);
            return;
        }
        audioSource.PlayOneShot(hitSound);
        stateMachine.ChangeState(StateName.ENEMY_HIT);

        var skillGauge = Player.Instance.weaponManager.Weapon.CurrentSkillGauge;
        Player.Instance.weaponManager.Weapon.CurrentSkillGauge = Mathf.Clamp(++skillGauge, 0, BaseWeapon.MAX_SKILL_GAUGE);
    }

    protected void CalculateAliveOrMoving()
    {
        if (agent.enabled)
        {
            isAlived = agent.velocity.sqrMagnitude >= 0.1f * 0.1f && agent.remainingDistance <= agent.stoppingDistance + 0.1f;
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

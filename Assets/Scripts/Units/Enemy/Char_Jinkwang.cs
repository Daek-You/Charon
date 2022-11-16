using CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Jinkwang : Enemy
{
    private float chargeTimer = 0.0f;
    public float ChargeTimer { get { return chargeTimer; } set { chargeTimer = value; } }
    private bool IsSecondAttack { get; set; }
    public bool IsSecondAttackDone { get; set; }

    private Vector3 direction;
    public Vector3 Direction { get { return direction; } set { direction = value; } }

    GameObject effect;

    [SerializeField]
    private GameObject farAttackEffect;
    [SerializeField]
    private GameObject closeAttackEffect;

    void OnEnable()
    {
        currentHP = maxHP;
        IsSecondAttack = false;
        IsSecondAttackDone = false;
        //skinnedMeshRenderer.material.color = originMaterial.color;
        stateMachine?.ChangeState(StateName.ENEMY_CHARGE);
    }

    void Start()
    {
        InitSettings();
        Target = Player.Instance.transform;

        if (effectSounds.ContainsKey(SoundType.DIE) || effectSounds.ContainsKey(SoundType.HIT))
            return;

        AudioClip clip = Resources.Load<AudioClip>("Sounds/EffectSounds/Enemy/Char_Private_K/Sound_Eff_Char_Private_K_Die");
        effectSounds.Add(SoundType.DIE, clip);

        clip = Resources.Load<AudioClip>("Sounds/EffectSounds/Enemy/Sound_Eff_EnemyHit");
        effectSounds.Add(SoundType.HIT, clip);

        transform.LookAt(Target.transform.position);
        IsBoss = true;

        stateMachine.AddState(StateName.ENEMY_CHARGE, new EnemyChargeState(this));
        stateMachine.AddState(StateName.ENEMY_CHARGE_HIT, new EnemyChargeHitState(this));
        stateMachine.AddState(StateName.ENEMY_CLOSE_SKILL, new EnemyCloseSkillState(this));
        stateMachine.AddState(StateName.ENEMY_FAR_SKILL, new EnemyFarSkillState(this));

        stateMachine?.ChangeState(StateName.ENEMY_CHARGE);
    }

    public void MoveForAttack()
    {
        if (!IsSecondAttack)
            rigidBody.AddForce(direction * 9f, ForceMode.Impulse);

        animator.SetFloat("AttackSpeed", 1f);
    }

    public void StopForAttack()
    {
        if (!IsSecondAttack)
            animator.SetFloat("AttackSpeed", 10f);
        else
            rigidBody.velocity = Vector3.zero;
            
    }

    public void JumpForAttack()
    {
        rigidBody.AddForce(direction * 3f, ForceMode.Impulse);
    }

    public void StopJumpForAttack()
    {
        rigidBody.velocity = Vector3.zero;
    }

    public void CheckSecondAttack()
    {
        if (!IsSecondAttack)
        {
            IsSecondAttack = true;
            return;
        }

        IsSecondAttackDone = true;
        weapon.StopAttack();

        if (attackDelayCoroutine != null)
            StopCoroutine(attackDelayCoroutine);
        attackDelayCoroutine = StartCoroutine(CorSecondAttack());
    }

    private IEnumerator CorSecondAttack()
    {
        float timer = 0f;
        stateMachine.ChangeState(StateName.ENEMY_CHARGE);
        isCooltimeDone = false;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer >= attackDelay)
            {
                EnemyFarSkillState skillState = stateMachine.GetState(StateName.ENEMY_FAR_SKILL) as EnemyFarSkillState;
                skillState.IsAttack = false;
                skillState.isCheckedPlayerPosition = false;
                IsSecondAttack = false;

                isCooltimeDone = true;
                break;
            }

            yield return null;
        }
    }

    public void PlayFarAttackEffect()
    {
        if (IsSecondAttackDone)
            return;

        effect = Instantiate(farAttackEffect);

        effect.transform.position = transform.position;
        effect.transform.rotation = Quaternion.LookRotation(direction);
        effect.GetComponent<ParticleSystem>().Play();
        effect.GetComponent<Rigidbody>().AddForce(direction * 9f, ForceMode.Impulse);
    }

    public void PlayCloseAttackEffect()
    {
        effect = Instantiate(closeAttackEffect);

        effect.transform.position = transform.position + direction * 7f;
        effect.transform.rotation = Quaternion.LookRotation(direction);
        effect.GetComponent<ParticleSystem>().Play();
    }

    public void StopAttackEffect()
    {
        if (effect == null)
            return;

        effect.GetComponent<ParticleSystem>().Stop();
    }
}

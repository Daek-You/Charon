using CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    private Color originColor;                           /// 쿨타임 시각용 테스트 변수
    private DashState dashState;
    private DashAttackState dashAttackState;
    private AttackState attackState;
    private Coroutine dashCoolTimeCoroutine;
    private SkinnedMeshRenderer skinnedMeshRenderer;     /// 쿨타임 시각용 테스트 변수


    #region #Unity Functions
    void Update()
    {
        if (attackState.IsAttack)
        {
            Vector3 velocity = Player.Instance.rigidBody.velocity;
            Player.Instance.rigidBody.velocity = new Vector3(velocity.x, 0f, velocity.z);
        }
    }


    void Start()
    {
        dashState = Player.Instance.stateMachine.GetState(StateName.DASH) as DashState;
        dashAttackState = Player.Instance.stateMachine.GetState(StateName.DASH_ATTACK) as DashAttackState;
        attackState = Player.Instance.stateMachine.GetState(StateName.ATTACK) as AttackState;
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = skinnedMeshRenderer.material.color;
    }
    #endregion

    public void OnStartAttack()
    {
        BaseWeapon currentWeapon = Player.Instance.weaponManager.Weapon;
        GameObject effect = Instantiate(currentWeapon.defaultAttackEffs[currentWeapon.ComboCount - 1]);

        Vector3 targetDirection = Player.Instance.Controller.MouseDirection;
        effect.transform.position = Player.Instance.effectGenerator.position;

        Vector3 secondAttackAdjustAngle = currentWeapon.ComboCount == 2 ? new Vector3(0f, -90f, 0f) : Vector3.zero;
        effect.transform.rotation = Quaternion.LookRotation(targetDirection);
        effect.transform.eulerAngles += secondAttackAdjustAngle;
        effect.GetComponent<ParticleSystem>().Play();
    }

    public void OnStartDashAttack()
    {
        BaseWeapon currentWeapon = Player.Instance.weaponManager.Weapon;
        GameObject effect = Instantiate(currentWeapon.dashAttackEffs);
        Vector3 targetDirection = Player.Instance.Controller.MouseDirection;

        effect.transform.position = Player.Instance.effectGenerator.position;
        effect.transform.rotation = Quaternion.LookRotation(targetDirection);
        effect.GetComponent<ParticleSystem>().Play();
    }


    public void OnFinishedAttack()
    {
        attackState.IsAttack = false;
        Player.Instance.animator.SetBool("IsAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }
    
    public void OnFinishedDashAttack()
    {
        dashAttackState.IsDashAttack = false;
        dashState.CanDashAttack = false;
        Player.Instance.animator.SetBool("IsDashAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnCanDashAttack()
    {
        dashState.CanDashAttack = true;
    }

    public void OnChangeDashToDashAttack()
    {
        if (dashAttackState.IsPressDashAttack)
        {
            dashAttackState.IsPressDashAttack = false;
            dashState.inputDirectionBuffer.Clear();
            Player.Instance.stateMachine.ChangeState(StateName.DASH_ATTACK);
        }
    }

    public void OnFinishedDash()
    {
        if (!dashAttackState.IsDashAttack)
        {
            dashState.CanDashAttack = false;

            if (dashState.inputDirectionBuffer.Count > 0)
            {
                Player.Instance.stateMachine.ChangeState(StateName.DASH);
                return;
            }

            Player.Instance.stateMachine.ChangeState(StateName.MOVE);

            if (dashCoolTimeCoroutine != null)
                StopCoroutine(dashCoolTimeCoroutine);
            dashCoolTimeCoroutine = StartCoroutine(CheckDashReInputLimitTime(dashState.dashCooltime));
        }
    }

    public void OnCanAddToDashInputBuffer()
    {
        dashState.CanAddInputBuffer = true;
    }

    private IEnumerator CheckDashReInputLimitTime(float limitTime)
    {
        float timer = 0f;
        skinnedMeshRenderer.material.color = Color.red;

        while (true)
        {
            timer += Time.deltaTime;
            
            if(timer > limitTime)
            {
                dashState.Reset();
                skinnedMeshRenderer.material.color = originColor;
                break;
            }
            yield return null;
        }
    }
}

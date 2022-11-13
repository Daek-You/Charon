using CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Jinkwang : Enemy
{
    void Start()
    {
        InitSettings();

        IsBoss = true;
        transform.LookAt(target.transform.position);

        stateMachine.DeleteState(StateName.ENEMY_MOVE);
        stateMachine.DeleteState(StateName.ENEMY_ATTACK);

        stateMachine.AddState(StateName.ENEMY_CHARGE, new EnemyChargeState(this));
        stateMachine.AddState(StateName.ENEMY_CLOSE_SKILL, new EnemyCloseSkillState(this));
        // stateMachine.AddState(StateName.ENEMY_FAR_SKILL, new EnemyFarSkillState(this));
        stateMachine.AddState(StateName.ENEMY_STIFFNESS, new EnemyStiffnessState(this));

        stateMachine?.ChangeState(StateName.ENEMY_CHARGE);
    }
}

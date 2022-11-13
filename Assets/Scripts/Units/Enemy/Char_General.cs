using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_General : Enemy
{

    void OnEnable()
    {
        currentHP = maxHP;
        stateMachine?.ChangeState(CharacterController.StateName.ENEMY_MOVE);
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
    }
}
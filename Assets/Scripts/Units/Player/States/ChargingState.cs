using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CharacterController
{
    public class ChargingState : BaseState
    {
        public bool IsCharging { get; set; }
        public float ChargingGauge { get; private set; }
        public const float MAX_CHARGING_TIME = 3f;
        private float chargingTime;

        public override void OnEnterState()
        {
            IsCharging = true;
            string name = Player.Instance.weaponManager.Weapon.Name;
            if(Player.Instance._AnimationEventHandler.myWeaponEffects.TryGetValue(name, out IEffect effect))
            {
                effect.PlayCharingEffect();
            }
            
            if(Player.Instance._AnimationEventHandler.mySounds.TryGetValue(name, out ISound sound))
            {
                sound.PlayChargingSound();
            }

            chargingTime = 0f;
            ChargingGauge = 0f;
        }

        public override void OnExitState()
        {
            Player.Instance.Controller.IsChargingAction = false;
            IsCharging = false;
            Player.Instance.audioSource.Stop();
            Player.Instance.weaponManager.Weapon.GetComponent<IEffect>().DestroyEffect();
        }

        public override void OnFixedUpdateState() { }

        public override void OnUpdateState()
        {
            if(chargingTime > MAX_CHARGING_TIME)
            {
                Player.Instance.stateMachine.ChangeState(StateName.MOVE);
                return;
            }

            chargingTime += Time.deltaTime;
            ChargingGauge = CalculateDamageMultiplier(chargingTime);
        }

        private float CalculateDamageMultiplier(float currentChargingTime)
        {
            if (currentChargingTime >= 1.25f && currentChargingTime < 1.5f)
                return 1.25f;
            else if (currentChargingTime >= 1.5f && currentChargingTime < 1.75f)
                return 1.5f;
            else if (currentChargingTime >= 1.75f && currentChargingTime < 2f)
                return 1.75f;
            else if (currentChargingTime >= 2f)
                return 2f;
            return 1;
        }
    }
}


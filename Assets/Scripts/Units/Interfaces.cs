namespace CharacterController
{
    public interface IEffect
    {
        public void PlayComboAttackEffects();
        public void PlayDashAttackEffect();
        public void PlayCharingEffect();
        public void DestroyEffect();
        public void PlayChargingAttackEffect();
        public void PlaySkillEffect();
    }

    public interface IHittable
    {
        void Damaged(float damage);
    }
}
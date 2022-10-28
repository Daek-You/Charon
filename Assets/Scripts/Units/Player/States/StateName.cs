namespace CharacterController
{
    public enum StateName
    {
        MOVE = 100,
        DASH,
        ATTACK,
        DASH_ATTACK,
        CHARGING,
        CHARGING_ATTACK,
        SKILL
    }
}


public interface IEffect
{
    public void PlayComboAttackEffects();
    public void PlayDashAttackEffect();
    public void PlayCharingEffect();
    public void DestroyEffect();
    public void PlayChargingAttackEffect();
    public void PlaySkillEffect();
}
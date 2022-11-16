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
        SKILL,
        HIT,
        DIE,

        ENEMY_MOVE = 10000,
        ENEMY_ATTACK,
        ENEMY_HIT,
        ENEMY_DIE,
        ENEMY_STAY,
        ENEMY_CHARGE,
        ENEMY_CHARGE_HIT,
        ENEMY_CLOSE_SKILL,
        ENEMY_FAR_SKILL
    }
}
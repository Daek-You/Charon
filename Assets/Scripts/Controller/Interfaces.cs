using UnityEngine;

public interface IComponent<T>
{
    void UpdateComponent(T owner);
}

public interface IWeapon
{
    public void Attack(Controller owner, Vector3 mouseDirection);
    public void DashAttack(Controller owner, Vector3 mouseDirection);
    public void ChargeAttack(Controller owner, Vector3 mouseDirection);
    public void Skill(Controller owner, Vector3 mouseDirection);
    public void UltimateSkill(Controller owner, Vector3 mouseDirection);
}
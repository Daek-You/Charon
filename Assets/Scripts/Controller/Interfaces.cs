


public interface ICommand<T>
{
    void InputUpdate(T owner);
    void PhysicsUpdate(T owner);
}


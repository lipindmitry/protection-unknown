public interface IStateWithArgs<T> : IState
{
    void Enter(T parametr);
}


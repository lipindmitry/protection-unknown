using System;
using System.Collections.Generic;

public abstract class StateWithArgs<T> : IStateWithArgs<T>
{
    protected readonly StateMachine _stateMachine;
    protected readonly Unit _unit;

    public abstract IEnumerable<Type> AvailableNextStates { get; }

    protected StateWithArgs(StateMachine stateMachine, Unit unit)
    {
        _stateMachine = stateMachine;
        _unit = unit;
    }

    public abstract void Enter(T parametr);

    public abstract void Exit();

    public abstract void Tick(float deltaTime);

    public virtual void PermanentTick(float deltaTime)
    {
    }
}


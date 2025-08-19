using System;
using System.Collections.Generic;

public class DeathState : IStateWithArgs<EmptyArgs>
{
    public IEnumerable<Type> AvailableNextStates => new Type[] { };

    public void Enter(EmptyArgs parametr)
    {
    }

    public void Exit()
    {
    }

    public void PermanentTick(float deltaTime)
    {
    }

    public void Tick(float deltaTime)
    {
    }
}


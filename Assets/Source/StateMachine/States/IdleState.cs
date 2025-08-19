using System;
using System.Collections.Generic;

public class IdleState : IStateWithArgs<EmptyArgs>
{
    public IEnumerable<Type> AvailableNextStates => new Type[] 
    { 
        typeof(DeathState),
        typeof(StunState),
        typeof(MoveToPositionState),
        typeof(MoveToTargetState),
        typeof(PrepareAttackState),
        typeof(PrepareSkillState)
    };

    public void Enter(EmptyArgs emptyArgs)
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

public class EmptyArgs
{

}


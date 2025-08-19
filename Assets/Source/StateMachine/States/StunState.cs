using System;
using System.Collections.Generic;

public class StunState : StateWithArgs<StunStateArts>
{
    public override IEnumerable<Type> AvailableNextStates => new Type[]
    {
        typeof(IdleState),
        typeof(DeathState)
    };

    private float _remainingTime;

    public StunState(StateMachine stateMachine, Unit unit) 
        : base(stateMachine, unit)
    {
    }

    public override void Enter(StunStateArts args)
    {
        _remainingTime = args.Duration;
    }

    public override void Exit()
    {
        _remainingTime = 0f;
    }

    public override void Tick(float deltaTime)
    {
        _remainingTime -= deltaTime;
        if (_remainingTime <= 0f)
            _stateMachine.ChangeState<IdleState, EmptyArgs>();
    }
}

public class StunStateArts
{
    public float Duration { get; set; }
}

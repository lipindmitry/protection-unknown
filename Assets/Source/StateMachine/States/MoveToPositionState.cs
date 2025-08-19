using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPositionState : StateWithArgs<MoveToPositionArgs>, IMoveState
{
    public override IEnumerable<Type> AvailableNextStates => new Type[]
    {
        typeof(IdleState),
        typeof(DeathState),
        typeof(StunState),
        typeof(MoveToPositionState),
        typeof(PrepareAttackState),
        typeof(PrepareSkillState)
    };

    private float SQRT_DELTA = 0.1f;

    private readonly NavMeshAgentSync _navMeshAgent;
    private readonly MoveStopper _moveStopper;

    private Vector3 _targetPosition;

    public MoveToPositionState(StateMachine stateMachine, Unit unit) : base(stateMachine, unit)
    {
        _navMeshAgent = _unit.GetComponentInChildren<NavMeshAgentSync>()
            ?? throw new Exception($"No {nameof(NavMeshAgentSync)} component on {unit.name}");

        _moveStopper = _unit.GetComponentInChildren<MoveStopper>() 
            ?? throw new Exception($"No {nameof(MoveStopper)} component on {unit.name}.");
    }

    public override void Enter(MoveToPositionArgs args)
    {
        _targetPosition = args.Target;
        _navMeshAgent.SetDestination(_targetPosition);

        _moveStopper.MoveStoped += Stop;
        _moveStopper.StartMonitoring();
    }

    public override void Exit()
    {
        _moveStopper.MoveStoped -= Stop;
        _moveStopper.StopMonitoring();
        _navMeshAgent.ResetPath();
    }

    public override void Tick(float deltaTime)
    {
        if ((_unit.transform.position - _targetPosition).sqrMagnitude < SQRT_DELTA)
            Stop();
    }

    private void Stop()
    {
        _stateMachine.ChangeState<IdleState, EmptyArgs>();
    }
}

public class MoveToPositionArgs
{
    public Vector3 Target { get; set; }
}


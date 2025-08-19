using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetState : StateWithArgs<Unit>, IMoveState
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

    private const float DELTA = 0.1f;

    private readonly NavMeshAgentSync _navMeshAgent;
    private readonly MoveStopper _moveStopper;

    private Unit _target;

    private float _sqrRange;
    private float _counter;

    public MoveToTargetState(StateMachine stateMachine, Unit unit) 
        : base(stateMachine, unit)
    {
        _navMeshAgent = _unit.GetComponentInChildren<NavMeshAgentSync>()
            ?? throw new Exception($"No {nameof(NavMeshAgentSync)} component on {unit.name}");

        _moveStopper = _unit.GetComponentInChildren<MoveStopper>()
            ?? throw new Exception($"No {nameof(MoveStopper)} component on {unit.name}.");
    }

    public override void Enter(Unit target)
    {
        _target = target;
        _sqrRange = Mathf.Pow(_unit.Weapon.Range - DELTA, 2);
        _navMeshAgent.SetDestination(_target.transform.position);

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
        if ((_unit.transform.position - _target.transform.position).sqrMagnitude < _sqrRange)
        {
            _stateMachine.ChangeState<PrepareAttackState, Unit>(_target);
            return;
        }

        _counter += Time.deltaTime;
        if (_counter > 1)
        {
            _navMeshAgent.ResetPath();
            _navMeshAgent.SetDestination(_target.transform.position);
            _counter = 0;
        }
    }

    private void Stop()
    {
        _stateMachine.ChangeState<IdleState, EmptyArgs>();
    }
}


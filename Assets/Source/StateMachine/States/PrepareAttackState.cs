using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareAttackState : StateWithArgs<Unit>
{
    public override IEnumerable<Type> AvailableNextStates => new Type[]
    {
        typeof(AttackState),
        typeof(DeathState),
        typeof(StunState),
        typeof(MoveToPositionState),
        typeof(MoveToTargetState),
        typeof(PrepareAttackState),
        typeof(PrepareSkillState)
    };

    private Unit _target;

    private float _attackDelay;
    private float _attackInterval;

    public PrepareAttackState(StateMachine stateMachine, Unit unit) 
        : base(stateMachine, unit)
    { 
    }

    public override void Enter(Unit target)
    {
        _attackInterval = _unit.Weapon.GetPropertyValue(WeaponPropertyType.AttackInterval);
        _target = target;
        Debug.Log($"Attack interval {_attackInterval}");
    }

    public override void Exit()
    {
    }

    public override void Tick(float deltaTime)
    {
        if (!_unit.InRange(_target))
        {
            _unit.StartCoroutine(Move());
            //_stateMachine.ChangeState<MoveToTargetState, Unit>(_target);
            return;
        }

        if (_attackDelay <= 0)
        {
            _attackDelay = _attackInterval;
            _stateMachine.ChangeState<AttackState, Unit>(_target);
        }
    }

    private IEnumerator Move()
    {
        yield return null;

        _stateMachine.ChangeState<MoveToTargetState, Unit>(_target);
    }

    public override void PermanentTick(float deltaTime)
    {
        base.PermanentTick(deltaTime);

        if (_attackDelay > 0)
            _attackDelay -= deltaTime;
    }
}
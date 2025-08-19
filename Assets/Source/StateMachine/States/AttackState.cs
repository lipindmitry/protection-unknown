using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateWithArgs<Unit>
{
    public override IEnumerable<Type> AvailableNextStates => new Type[]
    {
        typeof(IdleState),
        typeof(DeathState),
        typeof(StunState),
        typeof(MoveToPositionState),
        typeof(PrepareAttackState),
        typeof(PrepareSkillState),
        typeof(MoveToTargetState)
    };

    private Unit _target;

    private float _damageDelay;
    private float _afterDamageDelay;
    private float _damageRemainingTime;
    private float _afterDamageWaiting;
    private bool _attacked;

    public AttackState(StateMachine stateMachine, Unit unit) 
        : base(stateMachine, unit)
    {
    }

    public override void Enter(Unit target)
    {
        _damageDelay = _unit.Weapon.GetPropertyValue(WeaponPropertyType.DamageDelay);
        _afterDamageDelay = _unit.Weapon.GetPropertyValue(WeaponPropertyType.AfterDamageDelay);
        _target = target;
        _attacked = false;

        if (_target.IsDead)
            _stateMachine.ChangeState<IdleState, EmptyArgs>();

        else if (_unit.InRange(_target))
            Attack();

        else
            _stateMachine.ChangeState<MoveToTargetState, Unit>(_target);
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {
        if (_target.IsDead)
        {
            _stateMachine.ChangeState<IdleState, EmptyArgs>();
            return;
        }

        if (_damageRemainingTime > 0)
            _damageRemainingTime -= Time.deltaTime;

        if (_afterDamageWaiting > 0)
            _afterDamageWaiting -= Time.deltaTime;

        if (_damageRemainingTime <= 0 && !_attacked)
        {
            _unit.DealDamage(_target);
            _afterDamageWaiting = _afterDamageDelay;
            _attacked = true;
        }

        if (_attacked && _afterDamageWaiting <= 0)
            _stateMachine.ChangeState<PrepareAttackState, Unit>(_target);
    }

    private void Attack()
    {
        _damageRemainingTime = _damageDelay;

        _unit.transform.DOLookAt(_target.transform.position, 0.2f)
            .SetEase(Ease.OutSine)
            .SetUpdate(UpdateType.Fixed);
    }
}


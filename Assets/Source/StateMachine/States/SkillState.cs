using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillState : StateWithArgs<SkillTargetArgs>
{
    public override IEnumerable<Type> AvailableNextStates => new Type[]
    {
        typeof(IdleState),
        typeof(DeathState),
        typeof(StunState),
    };

    private SkillTargetArgs _skillTargetArgs;

    private float _skillRemainingTime;
    private float _usedRamainingTime;
    private float _effectRemainingTime;
    private bool _used;
    private bool _effectSpawned;

    public Skill ActiveSkill => _skillTargetArgs.Skill;

    public SkillState(StateMachine stateMachine, Unit unit) 
        : base(stateMachine, unit)
    {
    }

    public override void Enter(SkillTargetArgs skillTargetArgs)
    {
        _skillTargetArgs = skillTargetArgs;
        _used = false;
        _effectSpawned = false;
        _skillRemainingTime = skillTargetArgs.Skill.SkillDuration;
        _usedRamainingTime = skillTargetArgs.Skill.UsedDelay;
        _effectRemainingTime = skillTargetArgs.Skill.EffectDelay;

        skillTargetArgs.ActivateSkill();
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        if (_skillRemainingTime > 0)
            _skillRemainingTime -= Time.deltaTime;
        else
            _stateMachine.ChangeState<IdleState, EmptyArgs>();

        if (_usedRamainingTime > 0)
            _usedRamainingTime -= Time.deltaTime;

        if (_effectRemainingTime > 0)
            _effectRemainingTime -= Time.deltaTime;

        if (_effectRemainingTime <= 0 && !_effectSpawned)
        {
            GameObject.Instantiate(_skillTargetArgs.Skill.Effect, _unit.transform.position + _skillTargetArgs.Skill.EffectOffset, _unit.transform.rotation);
            _effectSpawned = true;
        }

        if (_usedRamainingTime < 0 && !_used)
        {
            _skillTargetArgs.Skill.Use();
            _used = true;
        }
    }
}


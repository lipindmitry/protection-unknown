
using System;
using System.Collections.Generic;

public class PrepareSkillState : StateWithArgs<SkillTargetArgs>
{
    public override IEnumerable<Type> AvailableNextStates => new Type[]
    {
        typeof(SkillState),
        typeof(DeathState),
        typeof(StunState),
        typeof(MoveToPositionState),
        typeof(PrepareAttackState),
        typeof(PrepareSkillState)
    };

    private SkillTargetArgs _skillTargetArgs;
    private float _remainingPrepare;

    public PrepareSkillState(StateMachine stateMachine, Unit unit) 
        : base(stateMachine, unit)
    {
    }

    public override void Enter(SkillTargetArgs skillTargetArgs)
    {
        _skillTargetArgs = skillTargetArgs;
        _remainingPrepare = _skillTargetArgs.Skill.CastingTime;
    }

    public override void Exit()
    {
        _remainingPrepare = 0;
    }

    public override void Tick(float deltaTime)
    {
        _remainingPrepare -= deltaTime;
        if (_remainingPrepare <= 0)
            _stateMachine.ChangeState<SkillState, SkillTargetArgs>(_skillTargetArgs);
    }
}

public class SkillTargetArgs
{
    public Skill Skill { get; set; }
    public SkillArguments Arguments { get; set; }

    public void ActivateSkill()
    {
        Skill.Activate(Arguments);
    }
}


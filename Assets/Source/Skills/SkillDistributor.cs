using UnityEngine;

public class SkillDistributor : MonoBehaviour
{
    [SerializeField] private SkillView[] _skillViews;
    [SerializeField] private TargetCalculator _targetCalculator;
    
    private Player _player;
    private StateMachine _stateMachine;
    private Skill _activeSkill;

    public void Initialize(Player player)
    {
        _stateMachine = player.GetComponentInChildren<StateMachine>();

        int viewIndex = 0;
        foreach (var skillSlot in player.SkillSlots)
        {
            _skillViews[viewIndex].SetSkillSlot(skillSlot);
            _skillViews[viewIndex].SkillRequestedActivated += OnSkillRequestedActivated;
            viewIndex++;
        }
        _player = player;
    }

    private void OnSkillRequestedActivated(SkillSlot skillSlot)
    {
        if (skillSlot.Cooldown != 0)
            return;

        _activeSkill = skillSlot.ActiveSkill;
        
        var args = new SkillTargetArgs()
        {
            Skill = _activeSkill,
            Arguments = new SkillArguments()
            {
                Unit = _player
            }
        };

        if (_activeSkill.TargetType == TargetType.Self)
        {
            _stateMachine.ChangeState<PrepareSkillState, SkillTargetArgs>(args);
        }
        else
        {
            _targetCalculator.TargetDetected += OnTargetDetected;
            _targetCalculator.DetectSkillArguments(_activeSkill.TargetType, _player);
        }
    }

    private void OnTargetDetected(SkillArguments skillArguments)
    {
        var args = new SkillTargetArgs()
        {
            Skill = _activeSkill,
            Arguments = skillArguments
        };

        _stateMachine.ChangeState<PrepareSkillState, SkillTargetArgs>(args);
    }
}


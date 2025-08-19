using UnityEngine;

public class AnimationSwitcherByState : MonoBehaviour
{
    [SerializeField] private AnimationSync _animationSync;
    [SerializeField] private StateMachine _stateMachine;

    private void Awake()
    {
        _stateMachine.StateChanged += OnStateChanged;
    }

    private void OnStateChanged(IState newState, IState previousState)
    {
        if (previousState is not IdleState
            && (newState is DeathState || newState is StunState)
            || newState is IdleState)
            _animationSync.SetTrigger("Stop");

        if (newState is DeathState)
            _animationSync.SetTrigger("Die");

        else if (newState is AttackState)
            _animationSync.SetTrigger("Attack");

        else if (newState is StunState)
            _animationSync.SetTrigger("Stun");

        else if (newState is IMoveState && previousState is not IMoveState)
        {
            _animationSync.SetTrigger("Move");
            Debug.Log("Move");
        }

        else if (newState is PrepareAttackState && previousState is not AttackState && previousState is not PrepareAttackState)
        {
            _animationSync.SetTrigger("PrepareAttack");
            Debug.Log("PrepareAttack");
        }

        else if (newState is SkillState skillState)
        {
            var tier = skillState.ActiveSkill.Tier;
            if (tier == 1)
                _animationSync.SetTrigger("Skill");
            else if (tier == 2)
                _animationSync.SetTrigger("Skill2");
            else if (tier == 3)
                _animationSync.SetTrigger("Skill3");
        }
    }
}


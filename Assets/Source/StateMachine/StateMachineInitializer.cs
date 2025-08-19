using UnityEngine;

public class StateMachineInitializer : MonoBehaviour
{
    [SerializeField] private StateMachine _stateMachine;
    [SerializeField] private Unit _unit;

    private void Awake()
    {
        _stateMachine.AddState(new IdleState());
        _stateMachine.AddState(new DeathState());
        _stateMachine.AddState(new PrepareAttackState(_stateMachine, _unit));
        _stateMachine.AddState(new AttackState(_stateMachine, _unit));
        _stateMachine.AddState(new MoveToPositionState(_stateMachine, _unit));
        _stateMachine.AddState(new MoveToTargetState(_stateMachine, _unit));
        _stateMachine.AddState(new PrepareSkillState(_stateMachine, _unit));
        _stateMachine.AddState(new SkillState(_stateMachine, _unit));
        _stateMachine.AddState(new StunState(_stateMachine, _unit));

        _stateMachine.ChangeState<IdleState, EmptyArgs>();
    }
}


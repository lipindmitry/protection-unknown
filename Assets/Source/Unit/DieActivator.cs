using UnityEngine;

public class DieActivator : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private StateMachine _stateMachine;

    private void Start()
    {
        _unit.Health.Changed += OnHealthChanged;
    }

    private void OnHealthChanged(Parametr health)
    {
        if (health.Current == 0)
            _stateMachine.ChangeState<DeathState, EmptyArgs>();
    }
}


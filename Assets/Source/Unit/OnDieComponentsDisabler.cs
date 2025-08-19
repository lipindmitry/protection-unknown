using UnityEngine;
using UnityEngine.AI;

public class OnDieComponentDisabler : MonoBehaviour
{
    [SerializeField] private Collider[] _colliders;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Unit _unit;

    private void Start()
    {
        _unit.Died += OnUnitDied;
    }

    private void OnUnitDied(Unit unit)
    {
        foreach (Collider collider in _colliders)
            collider.enabled = false;

        _agent.enabled = false;
    }
}

using System;
using UnityEngine;

public class MoveToTargetCommand : MonoBehaviour
{
    [SerializeField] private NavMeshAgentSync _navMeshAgent;
    [SerializeField] private MoveStopper _moveStopper;
    [SerializeField] private Unit _unit;
    
    private Transform _target;
    private float _counter;
    private float _sqrRange;

    public bool IsMoving { get; private set; }

    public event Action MoveStarted;
    public event Action MoveFinished;

    public void MoveTo(Transform target, float range)
    {
        _target = target;
        IsMoving = true;
        _counter = 0;
        _sqrRange = range * range;
        _navMeshAgent.SetDestination(target.position);
        _moveStopper.MoveStoped -= Stop;
        _moveStopper.MoveStoped += Stop;
        _moveStopper.StartMonitoring();

        MoveStarted?.Invoke();
    }

    public void Stop()
    {
        if (IsMoving)
        {
            IsMoving = false;
            _navMeshAgent.ResetPath();
            _moveStopper.MoveStoped -= Stop;
            MoveFinished?.Invoke();
            _moveStopper.StopMonitoring();
        }
    }

    private void Update()
    {
        if (IsMoving)
        {
            if ((_unit.transform.position - _target.position).sqrMagnitude < _sqrRange)
            {
                IsMoving = false;
                MoveFinished?.Invoke();
                _navMeshAgent.ResetPath();
            }

            _counter += Time.deltaTime;
            if (_counter > 1)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.SetDestination(_target.position);
                _counter = 0;
            }
        }
    }
}


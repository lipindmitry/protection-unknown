using System;
using UnityEngine;

public class MoveToPositionCommand : MonoBehaviour
{
    [SerializeField] private NavMeshAgentSync _navMeshAgent;
    [SerializeField] private MoveStopper _moveStopper;
    [SerializeField] private Player _player;

    private Vector3 _target;
    private bool _isMoving;
    private float _sqrDelta = 0.1f;

    public event Action MoveStarted;
    public event Action MoveFinished;

    public void MoveTo(Vector3 target)
    {
        _target = target;
        _isMoving = true;
        _navMeshAgent.SetDestination(target);

        _moveStopper.MoveStoped -= OnMoveStoped;
        _moveStopper.MoveStoped += OnMoveStoped;
        _moveStopper.StartMonitoring();

        MoveStarted?.Invoke();
    }

    private void OnMoveStoped()
    {
        Stop();
    }

    public void Stop()
    {
        if (_isMoving)
        {
            _isMoving = false;
            _navMeshAgent.ResetPath();
            _moveStopper.MoveStoped -= OnMoveStoped;
            MoveFinished?.Invoke();
            _moveStopper.StopMonitoring();
        }
    }

    private void Update()
    {
        if (_isMoving)
        {
            if ((_player.transform.position - _target).sqrMagnitude < _sqrDelta)
            {
                Stop();
            }
        }
    }
}


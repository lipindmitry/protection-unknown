using System;
using UnityEngine;

public class MoveStopper : MonoBehaviour
{
    [SerializeField] private Unit _unit;

    private const float DELTA = 0.0001f;

    private Vector3 _lastPosition;
    private bool _monitoring;
    private int _lastPositionRepeat;

    public event Action MoveStoped;

    private void Start()
    {
        _lastPosition = _unit.transform.position;
    }

    public void StartMonitoring()
    {
        _monitoring = true;
        _lastPositionRepeat = -1;
    }

    public void StopMonitoring()
    {
        _monitoring = false;
    }

    private void Update()
    {
        if (!_monitoring)
            return;

        if (_lastPositionRepeat == 30 && (_lastPosition - _unit.transform.position).sqrMagnitude < DELTA)
        {
            _lastPositionRepeat = -1;
            MoveStoped?.Invoke();
            _monitoring = false;
        }

        if (_lastPositionRepeat == 30)
            _lastPositionRepeat = -1;

        _lastPositionRepeat++;

        if (_lastPositionRepeat == 0)
            _lastPosition = _unit.transform.position;
    }
}


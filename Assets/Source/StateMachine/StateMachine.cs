using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private readonly Dictionary<Type, IState> _states = new();
    private IState _currentState;

    public event ChangeStateDelegate StateChanged;

    public delegate void ChangeStateDelegate(IState newState, IState previousState);

    private void Update()
    {
        _currentState?.Tick(Time.deltaTime);
        foreach (var state in _states.Values)
            state.PermanentTick(Time.deltaTime);
    }

    public void AddState<TState>(TState state)
        where TState : IState
    {
        _states[typeof(TState)] = state;
    }

    public void ChangeState<TState, TArgs>(TArgs args = null)
        where TState : IStateWithArgs<TArgs>
        where TArgs : class
    {
        if (!_states.ContainsKey(typeof(TState)))
            throw new Exception($"No state {typeof(TState).Name}");

        var newState = (IStateWithArgs<TArgs>)_states[typeof(TState)];

        if (_currentState != null &&
            !_currentState.AvailableNextStates.Contains(typeof(TState)))
            return;

        var previousState = _currentState;

        _currentState?.Exit();
        _currentState = newState;
        newState.Enter(args);

        Debug.Log($"State {newState.GetType().Name} ({transform.parent.name})");

        StateChanged?.Invoke(newState, previousState);
    }

    // Состояние смерти
    // Состояние стана
    // Состояние использования скила
}


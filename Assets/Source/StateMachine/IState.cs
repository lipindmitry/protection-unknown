using System;
using System.Collections.Generic;

public interface IState
{
    void Exit();
    void Tick(float deltaTime);
    void PermanentTick(float deltaTime);

    IEnumerable<Type> AvailableNextStates { get; }
}


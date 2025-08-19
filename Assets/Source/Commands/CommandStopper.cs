using System;
using UnityEngine;

public class CommandStopper : MonoBehaviour
{
    public event Action Stoped;

    public bool Stopping {  get; private set; }

    public void Continue()
    {
        Stopping = false;
    }

    public void Stop()
    {
        Stopping = true;
        Stoped?.Invoke();
    }
}


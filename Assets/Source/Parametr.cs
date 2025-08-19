
using System;

public class Parametr
{
    public event Action<Parametr> Changed;

    private int _initial;
    private float _current;
    private float _previous;

    public int Initial => _initial;
    public float Current => _current;

    public float Delta { get; internal set; }

    public void Initialize(int initial)
    {
        _initial = initial;
        _current = initial;
    }

    public void Increase(float value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        _previous = _current;
        _current += value;
        _current = MathF.Min(_current, _initial);
        Delta = _previous - _current;

        Changed?.Invoke(this);
    }

    public void Decrease(float value)
    {
        if (value == 0)
            return;

        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        _previous = _current;
        _current -= value;
        _current = MathF.Max(_current, 0);
        Delta = _previous - _current;

        Changed?.Invoke(this);
    }

    public void Set(float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        Delta = _current - value;
        _current = value;
        Changed?.Invoke(this);
    }
}


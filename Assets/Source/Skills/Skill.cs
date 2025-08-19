using System;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [SerializeField] private float _cooldown;
    [SerializeField] private float _castingTime;
    [SerializeField] private float _usedDelay;
    [SerializeField] private bool _instantActivation;
    [SerializeField] private float _skillDuration;
    [SerializeField] private float _activationRange;
    [SerializeField] private TargetType _targetType;
    [SerializeField] private int _tier;
    [SerializeField] private Sprite _icon;

    [SerializeField] private float _effectDelay;
    [SerializeField] private GameObject _effect;
    [SerializeField] private Vector3 _effectOffset;

    public float Cooldown => _cooldown;
    public float CastingTime => _castingTime;
    public float UsedDelay => _usedDelay;
    public float EffectDelay => _effectDelay;
    public float SkillDuration => _skillDuration;
    public float Range => _activationRange;
    public bool InstantActivation => _instantActivation;
    public TargetType TargetType => _targetType;
    public int Tier => _tier;
    public Sprite Icon => _icon;
    public GameObject Effect => _effect;
    public Vector3 EffectOffset => _effectOffset;

    public event Action<Skill> Activated;

    public abstract void Activate(SkillArguments skillArguments);
    public abstract void Use();

    protected void ActivateEvent()
    {
        Activated?.Invoke(this);
    }
}

public enum TargetType
{
    None = 0,
    Enemy = 1,
    Friend = 2,
    Area = 3,
    Self = 4
}


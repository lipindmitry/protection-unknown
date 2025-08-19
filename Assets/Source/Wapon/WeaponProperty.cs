using System;
using UnityEngine;

[Serializable]
public class WeaponProperty
{
    [SerializeField] private WeaponPropertyType _type;
    [SerializeField] private float _value;

    public WeaponPropertyType Type => _type;
    public float Value => _value;
}


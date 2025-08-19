using System;
using UnityEngine;

[Serializable]
public class StartАttribute
{
    [SerializeField] private AttributeType _type;
    [SerializeField] private float _value;

    public AttributeType Type => _type;
    public float Value => _value;
}

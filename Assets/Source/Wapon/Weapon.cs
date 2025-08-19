using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Skill[] _skills;
    [SerializeField] private WeaponProperty[] _properties;

    public Skill[] Skills => _skills;

    public float RandomDamage => (GetPropertyValue(WeaponPropertyType.MaxDamage) - GetPropertyValue(WeaponPropertyType.MinDamage)) * Random.value + GetPropertyValue(WeaponPropertyType.MinDamage);
    public float Range => GetPropertyValue(WeaponPropertyType.Range);

    public float GetPropertyValue(WeaponPropertyType type)
    {
        return _properties.First(x => x.Type == type).Value;
    }
}


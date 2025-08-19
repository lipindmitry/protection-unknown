using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    
    private readonly Dictionary<IStatusEffect,float> _activeEffects = new ();

    private void Update()
    {
        foreach (var effect in _activeEffects.Keys.ToArray())
        {
            _activeEffects[effect] -= Time.deltaTime;
            if (_activeEffects[effect] <= 0)
            {
                effect.Remove(_unit);
                _activeEffects.Remove(effect);
            }
        }
    }

    public void Apply(IStatusEffect effect)
    {
        effect.Apply(_unit);
        _activeEffects.Add(effect, effect.Duration);
    }
}


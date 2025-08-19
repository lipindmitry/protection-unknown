using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skills/Slash")]
public class Slash : Skill
{
    [SerializeField] private float _damageMultiplier;
    [SerializeField] private float _damageRadius;
    [SerializeField] private LayerMask _layerMask;
    
    private SkillArguments _skillArguments;

    public override void Activate(SkillArguments skillArguments)
    {
        _skillArguments = skillArguments;
        ActivateEvent();
    }

    public override void Use()
    {
        var player = _skillArguments.Unit;
        var colliders = Physics.OverlapSphere(player.transform.position, _damageRadius, _layerMask);
        foreach (var collider in colliders)
        {
            var unit = collider.GetComponent<Unit>();
            if (unit != player)
                unit.TakeDamage(player.Weapon.RandomDamage * _damageMultiplier, player);
        }
    }
}


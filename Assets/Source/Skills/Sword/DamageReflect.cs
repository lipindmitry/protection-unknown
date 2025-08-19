using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skills/DamageReflect")]
public class DamageReflect : Skill
{
    [SerializeField] private DamageReflectEffect _damageReflectEffect;

    public override void Activate(SkillArguments skillArguments)
    {
        var statusEffects = skillArguments.Unit.GetComponent<StatusEffects>();
        statusEffects.Apply(_damageReflectEffect);

        ActivateEvent();
    }

    public override void Use()
    {
    }
}
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skills/Effects/DamageReflectEffect")]
public class DamageReflectEffect : ScriptableObject, IStatusEffect
{
    [SerializeField] private float _duration;

    public float Duration => _duration;

    public void Apply(Unit unit)
    {
        unit.DamageTaking += ReflectDamage;
    }

    public void Remove(Unit unit)
    {
        unit.DamageTaking -= ReflectDamage;
    }

    public void Tick(float deltaTime)
    {
    }


    private void ReflectDamage(AttackerDamage attackerDamage)
    {
        if (attackerDamage.Attacker != null)
            attackerDamage.Attacker.TakeDamage(attackerDamage.Damage, null); // null, чтобы избежать рекурсии

        attackerDamage.MultiplyDamage(0);
    }
}


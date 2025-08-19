public class AttackerDamage
{
    public float Damage { get; private set; }
    public Unit Attacker { get; }

    public AttackerDamage(float damage, Unit attacker)
    {
        Damage = damage;
        Attacker = attacker;
    }

    public void MultiplyDamage(float multiplier)
    {
        Damage = multiplier * Damage;
    }
}


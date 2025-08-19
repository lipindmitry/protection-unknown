using System;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private StartАttribute[] _startAttributes;
    [SerializeField] private Weapon _weapon;

    public Parametr Health { get; } = new();
    public Weapon Weapon => _weapon;
    public bool IsDead => Health.Current == 0;
    public SkillSlot[] SkillSlots { get; private set; } = new SkillSlot[] { new(), new(), new()};

    public event Action<Unit> Died;
    public event Action<AttackerDamage> DamageTaking;

    protected virtual void Awake()
    {
        var constitution = _startAttributes
            .FirstOrDefault(x => x.Type == AttributeType.Constitution)
            ?? throw new Exception($"Unit {name} don't have constitution");
        Health.Initialize((int)constitution.Value * 5);

        Health.Changed += OnHealthChanged;

        int slotIndex = 0;
        foreach (var skill in Weapon.Skills)
            SkillSlots[slotIndex++].SetSkill(skill);
    }

    private void Update()
    {
        foreach (var skillSlot in SkillSlots)
            skillSlot.Tick(Time.deltaTime);
    }

    private void OnHealthChanged(Parametr health)
    {
        if (IsDead)
            Died?.Invoke(this);
    }

    public void TakeDamage(float value, Unit attacker)
    {
        var attackerDamage = new AttackerDamage(value, attacker);
        DamageTaking?.Invoke(attackerDamage);

        Health.Decrease(attackerDamage.Damage);
    }

    public void DealDamage(Unit target)
    {
        target.TakeDamage(Weapon.RandomDamage, this);
    }

    public bool InRange(Unit enemy)
    {
        return (transform.position - enemy.transform.position).sqrMagnitude < Weapon.Range * Weapon.Range;
    }

    public bool InRange(Vector3 position, float distance)
    {
        return (transform.position - position).sqrMagnitude < distance * distance;
    }
}


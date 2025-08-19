using UnityEngine;

public class SkillSlot
{
    public Skill ActiveSkill { get; private set; }
    public float Cooldown { get; private set; }
    public float RemainingCooldown => Cooldown / ActiveSkill.Cooldown;

    public void SetSkill(Skill skill)
    {
        ActiveSkill = skill;
        ActiveSkill.Activated += OnSkillActivated;
    }

    public void Tick(float deltaTime)
    {
        if (Cooldown > 0)
        {
            Cooldown -= deltaTime;
            Cooldown = Mathf.Max(Cooldown, 0);
        }
    }

    private void OnSkillActivated(Skill skill)
    {
        Cooldown = skill.Cooldown;
    }
}


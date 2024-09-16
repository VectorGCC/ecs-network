using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SkillDescription
{
    [SerializeReference, SubclassSelector] public ISkillTargetSelector TargetSelector;
    [SerializeReference, SubclassSelector] public List<ISkillComponent> Components;
}

public interface ISkillTargetSelector
{
    public IUnit[] EvaluateTargets(Context context);
}

public interface IUnit
{
    public float Health { get; }

    public void Heal(float value);
    public void Damage(float value);
    public void SetInvisible(bool value);
}

public class Health
{
    public void Heal(float value)
    {
    }
}

public struct StatusEffect
{
    public float Duration;
}

public class Context
{
    public IUnit Hero { get; }
}

public class SelfTargetSelector : ISkillTargetSelector
{
    public IUnit[] EvaluateTargets(Context context)
    {
        return new[] {context.Hero};
    }
}

public interface ISkillComponent
{
}

[Serializable]
public class Heal : ISkillComponent
{
    public float Value;
    public float Duration;
}

[Serializable]
public class ManaCostComponent : ISkillComponent
{
    public int ManaCost;
}

public class TestGameDesign
{
    public void Start()
    {
        // var targets = EntityMask.GetInRange(skill.Range);
        // foreach (var target in targets)
        // InstantiateProjectile(skill.ProjectileComponent, target, onHit => {
        // target.health -= skill.DamageComponent.Value;
        // target.ApplyDamage(skill.DamageComponent);
        //   target.ApplyStatusEffect(skill.BurningComponent);
        // });
    }
}
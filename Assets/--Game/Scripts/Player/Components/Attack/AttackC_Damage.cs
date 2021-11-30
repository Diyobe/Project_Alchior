using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum AttackPhysicalType
{
    BLUNT,
    PIERCE,
    SLASH,
}

public enum AttackType
{
    PHYSICAL,
    MAGICAL
}

public enum Element
{
    NEUTRAL, // Pas d'élément à l'attaque, neutre face à tout type d'ennemi
    PYROS,   // Attaque de feu, efficace face aux ennemis natura mais peu efficace face aux ennemis hydros
    NATURA,  // Attaque de plante, efficace face aux ennemis terra mais peu efficace face aux ennemis Pyros
    TERRA,   // Attaque minérale (roche, sable, terre), efficace face aux ennemis electra mais peu efficace face aux ennemis Natura
    ELECTRA, // Attaque d'électricité, efficace face aux ennemis Aeros mais peu efficace face aux ennemis Terra
    AEROS,   // Attaque de vent, efficace face aux ennemis Hydros mais peu efficace face aux ennemis Electra
    HYDROS,  // Attaque d'eau, efficace face aux ennemis pyros mais peu efficace face aux ennemis Aeros
}

public class AttackC_Damage : AttackComponent
{
    [Title("Attack Power")]
    public float power;
    public float damageOnGuard;

    [Title("Damage Type")]
    public Element attackElement;
    public AttackType type;
    public AttackPhysicalType physicalType;

    [Title("Damage Modifiers")]
    [SerializeField] float physicalTypeEffectiveModifier = 1.15f;
    [SerializeField] float physicalTypeNotEffectiveModifier = 0.9f;
    float physicalTypeModifier = 1;
    [SerializeField] float elementalEffectiveModifier = 1.5f;
    [SerializeField] float elementalNotEffectiveModifier = 0.6f;
    float elementalModifier = 1;

    public override void StartComponent(CharacterBase user)
    {

    }

    public override void UpdateComponent(CharacterBase user)
    {

    }

    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        if (type == AttackType.PHYSICAL)
        {
            float attackPower = (user.CharacterData.level + 10) * user.CharacterData.GetAttack() * 1.5f;
            float damage = attackPower / target.CharacterData.GetDefense() * 2;
            target.CharacterData.TakeDamage(damage);
        }
        else
        {
            float attackPower = (user.CharacterData.level + 10) * user.CharacterData.GetSpecialAttack() * 1.5f;
            float damage = attackPower / target.CharacterData.GetSpecialDefense() * 2;
            target.CharacterData.TakeDamage(damage);
        }
        /*user.PowerGauge.AddPower(user.PowerGauge.powerGivenOnAttack);
        target.PowerGauge.AddPower(user.PowerGauge.powerGivenToHitTarget);*/
    }

    public override void OnParry(CharacterBase user, CharacterBase target)
    {

    }
    public override void OnGuard(CharacterBase user, CharacterBase target, bool guardRepel)
    {
        //if (guardRepel == true)
        //{
        //    float damage = (percentDamageOnGuard * user.Stats.AttackMultiplier.Value) * target.Stats.DefenseMultiplier.Value;
        //    target.Stats.TakeDamage(damage);
        //}
    }
    public override void OnClash(CharacterBase user, CharacterBase target)
    {

    }
    public override void EndComponent(CharacterBase user)
    {

    }
}

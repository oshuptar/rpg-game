using RPG_Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Enums;
using RPG_Game.Weapons;
using RPG_Game.Interfaces;
using System.Text.Json.Serialization;
using RPG_Game.Potions;

namespace RPG_Game.Controller;

// this class is used for the purpose of calculating default damage values for different kinds of weapons

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$discriminator")]
[JsonDerivedType(typeof(MagicAttackStrategy), "MagicAttackStrategy")]
[JsonDerivedType(typeof(StealthAttackStrategy), "StealthAttackStrategy")]
[JsonDerivedType(typeof(NormalAttackStrategy), "NormalAttackStrategy")]
public abstract class AttackStrategy : IAttackStrategy
{
    public virtual int AttackRequestHandler(MagicWeapon magicWeapon, IEntity entity, int Damage)
    {
        EntityStats entityStats = entity.GetEntityStats();
        return Damage + entityStats.Attributes[PlayerAttributes.Wisdom].GetCurrentValue() / 2;
    }
    public virtual int AttackRequestHandler(LightWeapon lightWeapon, IEntity entity, int Damage)
    {
        EntityStats entityStats = entity.GetEntityStats();
        int criticalDamageFactor = new Random().Next() % 100 < entityStats.Attributes[PlayerAttributes.Luck].GetCurrentValue() - 1 ? 2 : 1;
        return criticalDamageFactor*(Damage + entityStats.Attributes[PlayerAttributes.Dexterity].GetCurrentValue() / 2);
    }
    public virtual int AttackRequestHandler(HeavyWeapon heavyWeapon, IEntity entity, int Damage)
    {
        EntityStats entityStats = entity.GetEntityStats();
        return Damage + entityStats.Attributes[PlayerAttributes.Strength].GetCurrentValue() / 10 
            + entityStats.Attributes[PlayerAttributes.Aggression].GetCurrentValue()/5;

    }
    public virtual int DefenseRequestHandler(MagicWeapon magicWeapon, IEntity entity, int AttackDamage)
    {
        EntityStats entityStats = entity.GetEntityStats();
        return CalculateDodgingDefense(entityStats.Attributes[PlayerAttributes.Luck].GetCurrentValue()
            + entityStats.Attributes[PlayerAttributes.Dexterity].GetCurrentValue() / 5, AttackDamage);
    }
    public virtual int DefenseRequestHandler(HeavyWeapon heavyWeapon, IEntity entity, int AttackDamage)
    {
        EntityStats entityStats = entity.GetEntityStats();
        return CalculateDodgingDefense(entityStats.Attributes[PlayerAttributes.Luck].GetCurrentValue()
            + entityStats.Attributes[PlayerAttributes.Strength].GetCurrentValue() / 10, AttackDamage);
    }
    public virtual int DefenseRequestHandler(LightWeapon lightWeapon, IEntity entity, int AttackDamage)
    {
        EntityStats entityStats = entity.GetEntityStats();
        return CalculateDodgingDefense(entityStats.Attributes[PlayerAttributes.Luck].GetCurrentValue()
            + entityStats.Attributes[PlayerAttributes.Dexterity].GetCurrentValue() / 5, AttackDamage);
    }
    protected virtual int CalculateDodgingDefense(int chance,int AttackDamage)
    {
        int dodgeFactor = new Random().Next() % 99 < chance ? 1 : 0;
        int deffenseValue = dodgeFactor * AttackDamage;
        return (deffenseValue == 0) ? AttackDamage - Math.Max(0, (AttackDamage - chance)) : deffenseValue;
    }
}
public class MagicAttackStrategy : AttackStrategy
{
    public override int AttackRequestHandler(MagicWeapon magicWeapon, IEntity entity, int Damage)
    {
        return base.AttackRequestHandler(magicWeapon, entity, Damage);
    }
    public override int AttackRequestHandler(LightWeapon lightWeapon, IEntity entity, int Damage)
    {
        return 1;
    }
    public override int AttackRequestHandler(HeavyWeapon heavyWeapon, IEntity entity, int Damage)
    {
        return 1;
    }
    public override int DefenseRequestHandler(MagicWeapon magicWeapon, IEntity entity, int AttackDamage)
    {
        EntityStats entityStats = entity.GetEntityStats();
        return AttackDamage - Math.Max(0, AttackDamage - 2 * entityStats.Attributes[PlayerAttributes.Luck].GetCurrentValue());
    }
    public override int DefenseRequestHandler(HeavyWeapon heavyWeapon, IEntity entity, int AttackDamage)
    {
        EntityStats entityStats = entity.GetEntityStats();
        return CalculateDodgingDefense(entityStats.Attributes[PlayerAttributes.Luck].GetCurrentValue(), AttackDamage);
    }
    public override int DefenseRequestHandler(LightWeapon lightWeapon, IEntity entity, int AttackDamage)
    {
        EntityStats entityStats = entity.GetEntityStats();
        return CalculateDodgingDefense(entityStats.Attributes[PlayerAttributes.Luck].GetCurrentValue(), AttackDamage);
    }
}

public class StealthAttackStrategy : AttackStrategy
{
    public override int AttackRequestHandler(MagicWeapon magicWeapon, IEntity entity , int Damage)
    {
        return 1;
    }
    public override int AttackRequestHandler(LightWeapon lightWeapon, IEntity entity, int Damage)
    {
        return 2*base.AttackRequestHandler(lightWeapon, entity, Damage);
    }
    public override int AttackRequestHandler(HeavyWeapon heavyWeapon, IEntity entity, int Damage)
    {
        return 2*base.AttackRequestHandler(heavyWeapon, entity, Damage);
    }
    public override int DefenseRequestHandler(MagicWeapon magicWeapon, IEntity entity, int AttackDamage)
    {
        return 0;
    }
    public override int DefenseRequestHandler(HeavyWeapon heavyWeapon, IEntity entity, int AttackDamage)
    {
        EntityStats entityStats = entity.GetEntityStats();
        return CalculateDodgingDefense(entityStats.Attributes[PlayerAttributes.Dexterity].GetCurrentValue() / 5, AttackDamage);
    }
    public override int DefenseRequestHandler(LightWeapon lightWeapon, IEntity entity, int AttackDamage)
    {
        EntityStats entityStats = entity.GetEntityStats();
        return CalculateDodgingDefense(entityStats.Attributes[PlayerAttributes.Dexterity].GetCurrentValue() / 10, AttackDamage);
    }
}
public class NormalAttackStrategy : AttackStrategy
{
    public override int AttackRequestHandler(MagicWeapon magicWeapon, IEntity entity, int Damage)
    {
        return 1;
    }
    public override int AttackRequestHandler(LightWeapon lightWeapon, IEntity entity, int Damage)
    {
        return base.AttackRequestHandler(lightWeapon, entity, Damage);
    }
    public override int AttackRequestHandler(HeavyWeapon heavyWeapon, IEntity entity, int Damage)
    {
        return base.AttackRequestHandler(heavyWeapon, entity, Damage);
    }
    public override int DefenseRequestHandler(MagicWeapon magicWeapon, IEntity entity, int AttackDamage)
    {
        return base.DefenseRequestHandler(magicWeapon, entity, AttackDamage);
    }
    public override int DefenseRequestHandler(HeavyWeapon heavyWeapon, IEntity entity, int AttackDamage)
    {
        return base.DefenseRequestHandler(heavyWeapon ,entity, AttackDamage);
    }
    public override int DefenseRequestHandler(LightWeapon lightWeapon, IEntity entity, int AttackDamage)
    {
        return base.DefenseRequestHandler(lightWeapon ,entity, AttackDamage);
    }
}
using RPG_Game.Controller;
using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Model.Entities;
using RPG_Game.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$discriminator")]
[JsonDerivedType(typeof(Sword), "Sword")]
[JsonDerivedType(typeof(Dagger), "Dagger")]
[JsonDerivedType(typeof(Hammer), "Hammer")]
[JsonDerivedType(typeof(PowerWeaponDecorator), "PowerWeaponDecorator")]
[JsonDerivedType(typeof(WeaponDecorator), "WeaponDecorator")]
public abstract class Weapon : Item
{
    //[JsonInclude]
    public abstract int Damage { get; }
    // THis field will be only accessed inside weapon classes, hence we can store it here
    //In case I need to provide Attack method for all IItem classes, I should move it there
    //[JsonInclude]
    public abstract int RadiusOfAction { get; }
    public abstract void DispatchAttack(AttackStrategy strategy, Entity? source, List<Entity>? target, int damage);
    public Weapon(): base() { }
}
public abstract class LightWeapon : Weapon
{
    public override int Capacity => base.Capacity;
    public LightWeapon(): base() { }
}
public abstract class HeavyWeapon : Weapon
{
    public override int Capacity => 2;
    public HeavyWeapon(): base() { }
}
public abstract class MagicWeapon : Weapon
{ 
    public override int Capacity => base.Capacity;
    public MagicWeapon() : base() { }
}




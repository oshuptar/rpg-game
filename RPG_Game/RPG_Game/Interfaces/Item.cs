using RPG_Game.Controller;
using RPG_Game.Currencies;
using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Model.Entities;
using RPG_Game.Potions;
using RPG_Game.Weapons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$discriminator")]
[JsonDerivedType(typeof(Weapon), "Weapon")]
[JsonDerivedType(typeof(Potion), "Potion")]
[JsonDerivedType(typeof(HealPotion), "HealPotion")]
[JsonDerivedType(typeof(TemporaryPotion), "TemporaryPotion")]
[JsonDerivedType(typeof(DexterityPotion), "DexterityPotion")]
[JsonDerivedType(typeof(EternalPotion), "EternalPotion")]
[JsonDerivedType(typeof(AggressionPotion), "AggressionPotion")]
[JsonDerivedType(typeof(Dagger), "Dagger")]
[JsonDerivedType(typeof(Hammer), "Hammer")]
[JsonDerivedType(typeof(Sword), "Sword")]
[JsonDerivedType(typeof(HeavyWeapon), "HeavyWeapon")]
[JsonDerivedType(typeof(LightWeapon), "LightWeapon")]
[JsonDerivedType(typeof(MagicWeapon), "MagicWeapon")]
[JsonDerivedType(typeof(PowerWeaponDecorator), "PowerWeaponDecorator")]
[JsonDerivedType(typeof(WeaponDecorator), "WeaponDecorator")]
[JsonDerivedType(typeof(ItemDecorator), "ItemDecorator")]
[JsonDerivedType(typeof(LuckItemDecorator), "LuckItemDecorator")]
[JsonDerivedType(typeof(Gold), "Gold")]
[JsonDerivedType(typeof(Coin), "Coin")]
public abstract class Item : ICopyable
{
    [JsonInclude]
    public abstract string Name { get; }
    [JsonInclude]
    public virtual int Capacity => 1;
    [JsonInclude]
    public abstract string Description { get; }
    public virtual void ApplyChanges(EntityStats entityStats) { }
    public virtual void RevertChanges(EntityStats entityStats) { }
    public virtual void Use(AttackStrategy strategy, Entity? source, List<Entity>? target) { }
    public virtual void PickUp(Entity entity) => ApplyChanges(entity.GetEntityStats());
    public virtual void Drop(Entity entity) => RevertChanges(entity.GetEntityStats());
    public abstract object Copy();

}

using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Interfaces;
using RPG_Game.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.Weapons;

public class WeaponDecorator : Weapon
{
    [JsonInclude]
    public Weapon Weapon { get; protected set; }
    public override string Description => Weapon.Description;
    public override int RadiusOfAction => Weapon.RadiusOfAction;
    public override int Capacity => Weapon.Capacity;
    public override int Damage => Weapon.Damage;
    public override string Name => Weapon.Name;
    public override void Use(AttackStrategy strategy, Entity? source, List<Entity>? target)
    {
        DispatchAttack(strategy, source, target, this.Damage);
    }

    public override void DispatchAttack(AttackStrategy strategy, Entity? source, List<Entity>? target, int damage)
    {
            Weapon.DispatchAttack(strategy, source, target, damage);
    }
    public WeaponDecorator(Weapon weapon)
    {
        this.Weapon = weapon;
    }
    public WeaponDecorator(): base() { }
    public override object Copy() => new WeaponDecorator((Weapon)   Weapon.Copy());
}
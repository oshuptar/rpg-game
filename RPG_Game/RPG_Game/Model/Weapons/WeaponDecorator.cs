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

//Decorator which is specific for a weapon
public class WeaponDecorator : Weapon
{
    [JsonInclude]
    protected Weapon weapon;
    public override string Description => weapon.Description;
    public override int RadiusOfAction => weapon.RadiusOfAction;
    public override int Capacity => weapon.Capacity;
    public override int Damage => weapon.Damage;
    public override string Name => weapon.Name;
    public override void Use(AttackStrategy strategy, Entity? source, List<Entity>? target)
    {
        DispatchAttack(strategy, source, target, this.Damage);
    }

    public override void DispatchAttack(AttackStrategy strategy, Entity? source, List<Entity>? target, int damage)
    {
        weapon.DispatchAttack(strategy, source, target, damage);
    }
    public WeaponDecorator(Weapon weapon)
    {
        this.weapon = weapon;
    }
    public override object Copy() => new WeaponDecorator((Weapon)weapon.Copy());
}

using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Weapons;

//Decorator which is specific for a weapon
public class WeaponDecorator : IWeapon
{
    protected IWeapon weapon;
    public override string Description => weapon.Description;
    public override int RadiusOfAction => weapon.RadiusOfAction;
    public override int Capacity => weapon.Capacity;
    public override int Damage => weapon.Damage;
    public override string Name => weapon.Name;

    public override void Use(AttackStrategy strategy, IEntity? source, List<IEntity>? target)
    {
        DispatchAttack(strategy, source, target, this.Damage);
    }

    public override void DispatchAttack(AttackStrategy strategy, IEntity? source, List<IEntity>? target, int damage)
    {
        weapon.DispatchAttack(strategy, source, target, damage);
    }
    public WeaponDecorator(IWeapon weapon)
    {
        this.weapon = weapon;
    }
    public override object Copy() => new WeaponDecorator((IWeapon)weapon.Copy());
}

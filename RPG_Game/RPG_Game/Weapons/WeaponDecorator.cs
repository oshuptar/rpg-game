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
    public override int Capacity => weapon.Capacity;
    public override int Damage => weapon.Damage;
    public override string Name => weapon.Name;

    public override string Description => weapon.Description;

    public WeaponDecorator(IWeapon weapon)
    {
        this.weapon = weapon;
    }
}

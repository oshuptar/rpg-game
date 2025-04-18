using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Weapons;

// + 5 to the weapon damage
public class PowerWeaponDecorator : WeaponDecorator 
{
    public PowerWeaponDecorator(IWeapon weapon) : base(weapon) { }
    public override string Name => weapon.Name + "(Increased Damage)";
    public override int Damage => weapon.Damage + 5;
    public override object Copy() => new PowerWeaponDecorator((IWeapon)weapon.Copy());
}

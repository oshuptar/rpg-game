using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public class PowerWeaponDecorator : WeaponDecorator // + 5 to the weapon damage
{
    public PowerWeaponDecorator(IWeapon weapon) : base(weapon) { }
    public override string Name => weapon.Name + "(Increased Damage)";
    public override int Damage => weapon.Damage + 5;
}

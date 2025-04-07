using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Weapons;

//two-handed weapon
public class Hammer : IWeapon
{
    public override int Damage => 10;
    public override string Name => "Hammer";
    //public override string ToString() => Name;
    public override int Capacity => 2;

    public override string Description => $"(Damage: {Damage}; Two-Handed)";

    public override object Copy() => new Hammer();
}

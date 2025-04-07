using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Weapons;

public class Dagger : IWeapon
{
    public override int Damage => 5;
    public override string Name => "Dagger";
    //public override string ToString() => Name;

    public override string Description => $"(Damage: {Damage}; One-Handed)";

    public override object Copy() => new Dagger();
}

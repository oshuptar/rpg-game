using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Weapons;

public class Dagger : IWeapon
{
    public int Damage => 5;
    public string Name => "Dagger";
    //public override string ToString() => Name;

    public string Description => $"(Damage: {Damage}; One-Handed)";
}

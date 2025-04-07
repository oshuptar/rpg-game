using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Weapons;

// Concrete Component
public class Sword : IWeapon
{
    public override int Damage => 7;
    public override string Name => "Sword";
    public override string Description => $"(Damage: {Damage}; One-Handed)";

    public override object Copy() => new Sword();
}

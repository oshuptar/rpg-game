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
    public int Damage => 7;
    public string Name => "Sword";
}

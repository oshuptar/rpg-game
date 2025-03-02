using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

//two-handed weapon
public class Hammer : IWeapon
{
    public int Damage => 10;
    public string Name => "Hammer";
    //public override string ToString() => Name;
    public int Capacity => 2;
}

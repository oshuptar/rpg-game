using RPG_Game.Entiities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;

public abstract class IWeapon : IItem
{
    public abstract int Damage { get; }

    //Default implementations
    //public void Attack(Player player)
    //{
    //    player.ReceiveDamage(Damage); // Access the damage value from the implementing class
    //}
}

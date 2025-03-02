using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

[Flags] // This allows combining the enum values using bitwise operators
public enum CellType // what if two objects of the same kind are placed on the same cell?
{
    Empty = 0,
    Wall = 1, // We cannot place items where the wall is placed, add validation
    Player = 2,
    Weapon1 = 4,
    Weapon2 = 8,
    Weapon3 = 16,
    Unusable1 = 32,
    Unusable2 = 64,
    Unusable3 = 128,
    Coin = 256,
    Gold = 512,

    // Or just add Other instead of Weapons, Unusable objects and Coin with Gold?
}


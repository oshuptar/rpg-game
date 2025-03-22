using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Enums;

[Flags] // This allows combining the enum values using bitwise operators
public enum CellType // what if two objects of the same kind are placed on the same cell?
{
    Empty = 0,
    Wall = 1, // We cannot place items where the wall is placed, add validation
    Player = 2,
    Item = 4,
    Enemy = 8,
}


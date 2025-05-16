using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Enums;

[Flags] // This allows combining the enum values using bitwise operators
public enum CellType
{
    Empty = 0,
    Wall = 1, 
    Player = 2,
    Item = 4,
    Enemy = 8,
}


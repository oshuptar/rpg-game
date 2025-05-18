using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public class HandsState
{
    public const int MaxCapacity = 2;
    public int Capacity { get; set; } = 0;
    public List<Item> Hands { get; } = new List<Item>();
}

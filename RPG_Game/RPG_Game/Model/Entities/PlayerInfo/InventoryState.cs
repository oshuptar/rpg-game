using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

// Implement as Dictionary
public class InventoryState
{
    public List<Item> Inventory { get; } = new List<Item>();
}

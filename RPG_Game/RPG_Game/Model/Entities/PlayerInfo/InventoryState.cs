using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

// Implement as Dictionary
public class InventoryState
{
    [JsonInclude]
    public List<Item> Inventory { get; private set; } = new List<Item>();
}

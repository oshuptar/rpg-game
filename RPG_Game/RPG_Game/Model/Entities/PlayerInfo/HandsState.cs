using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

// Fix HandState make more clean
public class HandsState
{
    public const int MaxCapacity = 2;
    [JsonInclude]
    public int Capacity { get; set; } = 0;
    [JsonInclude]
    public List<Item> Hands { get; private set; } = new List<Item>();
}

using RPG_Game.Entiities;
using RPG_Game.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;

// Do items need to contain attributes of a Player?
public interface IItem
{
    public string Name { get; }
    public int Capacity => 1; // default capacity for every item
    public void ApplyChanges(PlayerStats playerStats) { }
    public void RevertChanges(PlayerStats playerStats) { }

    public void Use() { }
}

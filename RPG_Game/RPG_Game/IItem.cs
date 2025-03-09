using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

// Do items need to contain attributes of a Player?
public interface IItem
{
    public string Name { get; }
    public int Capacity => 1; // default capacity for every item
    public void ApplyChanges(Player player) { }
    public void RevertChanges(Player player) { }
   // public string ToString() => Name; // Why this does not override?
}

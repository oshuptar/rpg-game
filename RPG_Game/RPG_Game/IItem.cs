using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

// Do items need to contain attributes of a Player?
public interface IItem
{
    public string Name { get; }
    public void PickUp(Player player) => player.PickUp(this);
    public void Drop(Player player){ }
    //public void ApplyChanges(Player player) { }
    //public void RevertChanges(Player player) { }
    public string ToString() => Name; // Why this does not override?
}

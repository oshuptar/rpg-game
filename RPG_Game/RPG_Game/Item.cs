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
    public void PickUp(Player player) => player.PickUp(this); // default implementation
    public string ToString() => Name; // Why this does not override?
}

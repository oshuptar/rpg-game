using RPG_Game.Entiities;
using RPG_Game.HelperClasses;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public class Inventory : StorageManager
{
    // implement inventory as dictionary
    public List<IItem> inventory { get; } = new List<IItem>();

    public IItem? DropFromInventory(Room room, int index, Player player)
    {
        IItem? item = Drop(room, index, inventory, player);
        return item;
    }

    public void PickUp(IItem? item, Player player)
    {
        if (item == null)
            return;
        inventory.Add(item);
        item.ApplyChanges(player.RetrievePlayerStats());
    }
}

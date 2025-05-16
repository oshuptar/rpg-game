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
    private InventoryState InventoryState { get; set; } = new InventoryState();
    public IItem? DropFromInventory(Room room, int index, Player player)
    {
        IItem? item = Drop(room, index, InventoryState.Inventory, player);
        return item;
    }
    public void EmptyInventory(Room room, Player player)
    {
        int itemCount = InventoryState.Inventory.Count;
        for(int i = 0; i < itemCount; i++)
            DropFromInventory(room, 0, player);
    }
    public void PickUp(IItem? item, Player player)
    {
        if (item == null)
            return;
        InventoryState.Inventory.Add(item);
        item.PickUp(player);
    }
    public void AddItem(IItem? item)
    {
        if (item == null) return;
        InventoryState.Inventory.Add(item);
    }
    public InventoryState GetInventoryState() => InventoryState;
}

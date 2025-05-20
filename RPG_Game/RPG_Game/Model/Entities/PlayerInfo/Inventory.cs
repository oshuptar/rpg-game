using RPG_Game.Entiities;
using RPG_Game.HelperClasses;
using RPG_Game.Interfaces;
using RPG_Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public class Inventory : StorageManager
{
    [JsonInclude]
    private InventoryState InventoryState { get; set; } = new InventoryState();
    public Item? DropFromInventory(AuthorityGameState room, int index, Player player)
    {
        Item? item = Drop(room, index, InventoryState.Inventory, player);
        return item;
    }
    public void EmptyInventory(AuthorityGameState room, Player player)
    {
        int itemCount = InventoryState.Inventory.Count;
        for(int i = 0; i < itemCount; i++)
            DropFromInventory(room, 0, player);
    }
    public void PickUp(Item? item, Player player)
    {
        if (item == null)
            return;
        InventoryState.Inventory.Add(item);
        item.PickUp(player);
    }
    public void AddItem(Item? item)
    {
        if (item == null) return;
        InventoryState.Inventory.Add(item);
    }
    public InventoryState GetInventoryState() => InventoryState;
}

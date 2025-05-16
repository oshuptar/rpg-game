using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.HelperClasses;
using RPG_Game.Entiities;
using RPG_Game.Entities;

namespace RPG_Game.Entities;
public class Hands : StorageManager
{
    private HandsState HandsState = new HandsState();
    public bool Equip(IItem? item, Player player ,bool isInInventory = true)
    {
        if (item == null || item.Capacity + HandsState.Capacity > HandsState.MaxCapacity)
            return false;

        HandsState.Hands.Add(item);
        HandsState.Capacity += item.Capacity;

        if (!isInInventory)
            item.PickUp(player);

        return true;
    }
    public IItem? UnEquip(int index)
    {
        if (HandsState.Hands.Count == 0)
            return null;

        IItem item = HandsState.Hands.ElementAt(index);
        HandsState.Hands.Remove(item);
        HandsState.Capacity -= item.Capacity;

        return item;
    }
    public IItem? DropFromHands(Room room, int index, Player player)
    {
        IItem? item = Drop(room, index, HandsState.Hands, player);

        if (item != null)
            HandsState.Capacity -= item.Capacity;
        return item;

    }
    public HandsState GetHandState() => HandsState;
}

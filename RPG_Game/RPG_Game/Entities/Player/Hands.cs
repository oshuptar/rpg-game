using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.HelperClasses;

using RPG_Game.Entiities;
namespace RPG_Game.Entities;

public class Hands : StorageManager
{
    public const int MaxCapacity = 2; // MaxCapacity of Hands
    public int Capacity { get; private set; } = 0;
    public List<IItem> hands { get; } = new List<IItem>(); //technicaly we can equip any item to use it later on like elixir

    public bool Equip(IItem? item, Player player ,bool isInInventory = true)
    {
        if (item == null || item.Capacity + Capacity > MaxCapacity)
            return false;

        hands.Add(item);
        Capacity += item.Capacity;

        if (!isInInventory)
            item.ApplyChanges(player.RetrieveEntityStats());

        return true;
    }
    public IItem? UnEquip(int index)
    {
        if (hands.Count == 0)
            return null;

        IItem item = hands.ElementAt(index);
        hands.Remove(item);
        Capacity -= item.Capacity;

        return item;
    }

    public IItem? DropFromHands(Room room, int index, Player player)
    {
        IItem? item = Drop(room, index, hands, player);

        if (item != null)
            Capacity -= item.Capacity;
        return item;

    }
}

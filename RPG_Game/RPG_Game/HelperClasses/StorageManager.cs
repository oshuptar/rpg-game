using RPG_Game.Entiities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.HelperClasses;

public abstract class StorageManager
{
    public IItem? Drop(Room room, int index, List<IItem> list, Player player)
    {
        IItem? item = Remove(index, list);
        if (item != null)
        {
            room.AddItem(item, (player.Position.x, player.Position.y));
            item.RevertChanges(player);
        }
        return item;
    }

    public IItem? Retrieve(int index, List<IItem> list)
    {
        if (list.Count == 0)
            return null;
        IItem item = list.ElementAt(index);
        return item;
    }

    public IItem? Remove(int index, List<IItem> list)
    {
        IItem? item = Retrieve(index, list);
        if (item != null)
            list.RemoveAt(index);
        return item;
    }
}

using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Interfaces;
using RPG_Game.Model;
using RPG_Game.Model.Entities;
using RPG_Game.Potions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.HelperClasses;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$discriminator")]
[JsonDerivedType(typeof(Hands), "Hands")]
[JsonDerivedType(typeof(Inventory), "Inventory")]
public abstract class StorageManager
{
    public Item? Drop(AuthorityGameState room, int index, List<Item> list, Entity entity)
    {
        Item? item = Remove(index, list);
        if (item != null)
        {
            room.AddItem(item, entity.Position);
            item.Drop(entity);
        }
        return item;
    }
    public Item? Retrieve(int index, List<Item> list)
    {
        if (list.Count == 0)
            return null;
        Item item = list.ElementAt(index);
        return item;
    }
    public Item? Remove(int index, List<Item> list)
    {
        Item? item = Retrieve(index, list);
        if (item != null)
            list.RemoveAt(index);
        return item;
    }
}

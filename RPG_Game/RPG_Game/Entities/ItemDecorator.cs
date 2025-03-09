using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entiities;

public class ItemDecorator : IItem
{
    protected IItem item;
    public virtual string Name => item.Name;
    public virtual int Capacity => item.Capacity; //reccursively callls this function until it reaches some concrete object
    public virtual void ApplyChanges(Player player)
    {
        item.ApplyChanges(player);
    }
    public virtual void RevertChanges(Player player)
    {
        item.RevertChanges(player);
    }
    public ItemDecorator(IItem item)
    {
        this.item = item;
    }
}

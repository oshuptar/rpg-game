using RPG_Game.Entities;
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
    public virtual void ApplyChanges(PlayerStats playerStats) => item.RevertChanges(playerStats);
    public virtual void RevertChanges(PlayerStats playerStats) => item.RevertChanges(playerStats);
   
    public ItemDecorator(IItem item)
    {
        this.item = item;
    }
}

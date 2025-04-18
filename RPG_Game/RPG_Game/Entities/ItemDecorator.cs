using RPG_Game.Entities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entiities;

public class ItemDecorator : IItem
{
    protected IItem item;
    public override string Name => item.Name;
    public override int Capacity => item.Capacity; //reccursively callls this function until it reaches some concrete object
    public override void ApplyChanges(EntityStats entityStats) => item.ApplyChanges(entityStats);
    public override void RevertChanges(EntityStats entityStats) => item.RevertChanges(entityStats);

    public override void Use(IEntity? source, List<IEntity>? target)
    {
        item.Use(source, target);
    }
    public override string Description => item.Description;
   
    public ItemDecorator(IItem item)
    {
        this.item = item;
    }

    public override object Copy() => new ItemDecorator((IItem)item.Copy());
}

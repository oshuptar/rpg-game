using RPG_Game.Entities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Controller;
using System.Text.Json.Serialization;
using RPG_Game.Model.Entities;

namespace RPG_Game.Entiities;

public class ItemDecorator : Item
{
    [JsonInclude]
    protected Item item;
    public override string Name => item.Name;
    public override int Capacity => item.Capacity; //reccursively callls this function until it reaches some concrete object
    public override void ApplyChanges(EntityStats entityStats) => item.ApplyChanges(entityStats);
    public override void RevertChanges(EntityStats entityStats) => item.RevertChanges(entityStats);
    public override void Use(AttackStrategy strategy, Entity? source, List<Entity>? target)
    {
        item.Use(strategy,source, target);
    }
    public override string Description => item.Description;
   
    public ItemDecorator(Item item)
    {
        this.item = item;
    }
    public override object Copy() => new ItemDecorator((Item)item.Copy());
}

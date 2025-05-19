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
    public Item Item { get; private set; }
    public override string Name => Item.Name;
    public override int Capacity => Item.Capacity; //reccursively callls this function until it reaches some concrete object
    public override void ApplyChanges(EntityStats entityStats) => Item.ApplyChanges(entityStats);
    public override void RevertChanges(EntityStats entityStats) => Item.RevertChanges(entityStats);
    public override void Use(AttackStrategy strategy, Entity? source, List<Entity>? target)
    {
        Item.Use(strategy,source, target);
    }
    public override string Description => Item.Description;
    public ItemDecorator() { }
    public ItemDecorator(Item item)
    {
        this.Item = item;
    }
    public override object Copy() => new ItemDecorator((Item)Item.Copy());
}

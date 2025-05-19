using RPG_Game.Entities;
using RPG_Game.Interfaces;
using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entiities;

public class LuckItemDecorator : ItemDecorator
{
    public LuckItemDecorator() { }
    public LuckItemDecorator(Item item) : base(item) { }
    public override string Name => "(Lucky) " + Item.Name;
    public override void ApplyChanges(EntityStats entityStats)
    {
        entityStats.ModifyEntityAttribute(PlayerAttributes.Luck, 2);
        Item.ApplyChanges(entityStats);
    }
    public override void RevertChanges(EntityStats entityStats)
    {
        entityStats.ModifyEntityAttribute(PlayerAttributes.Luck, -2); // the name is hardcoded, think about the way to fix this
        Item.RevertChanges(entityStats);
    }
    public override object Copy() => new LuckItemDecorator((Item)Item.Copy());
}
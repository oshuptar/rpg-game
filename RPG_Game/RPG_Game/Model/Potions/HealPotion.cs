using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Controller;
using System.Text.Json.Serialization;
using RPG_Game.Model.Entities;

namespace RPG_Game.Potions;
public class HealPotion : EternalPotion
{
    public override string Name => $"Healing Potion {(IsDisposed ? "(Disposed)" : "")}";
    [JsonInclude]
    public static int HP => 5;
    public override void ApplyEffect(EntityStats? entityStats)
    {
        entityStats?.ModifyEntityAttribute(PlayerAttributes.Health, HP);
    }
    public override string Description => $"(Adds {HP} HP)";

    public override void Dispose(Entity? entity)
    {
        IsDisposed = true;
    }
    public override void Use(AttackStrategy strategy, Entity? source, List<Entity>? targets)
    {
        if (IsDisposed) return;

        ApplyEffect(source?.GetEntityStats());
        Dispose(source);
    }
    public override object Copy() => new HealPotion();
}

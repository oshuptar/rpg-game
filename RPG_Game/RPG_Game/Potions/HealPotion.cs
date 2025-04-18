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

namespace RPG_Game.Potions;

// Eternal potion
public class HealPotion : EternalPotion
{
    public override string Name => $"Healing Potion {(IsDisposed ? "(Disposed)" : "")}";

    public int HP = 5;
    public override void ApplyEffect(EntityStats? entityStats)
    {
        entityStats?.ModifyEntityAttribute(PlayerAttributes.Health, HP);
    }

    public override string Description => $"(Adds {HP} HP)";

    public override void Dispose(IEntity? entity)
    {
        IsDisposed = true;
    }

    public override void Use(AttackStrategy strategy, IEntity? source, List<IEntity>? targets)
    {
        if (IsDisposed) return;

        ApplyEffect(source?.RetrieveEntityStats());
        Dispose(source);
    }

    public override object Copy() => new HealPotion();
}

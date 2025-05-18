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

// The ability of hero dodging attacks
public class DexterityPotion : TemporaryPotion
{
    public override string Name => $"Dexterity Potion {(IsDisposed ? "(Disposed)" : "")}";
    protected override int ActiveTime { get; set; } = 0;
    public override int Lifetime => 10;
    [JsonInclude]
    public static int Dexterity => 10;
    public override string Description => $"(Adds {Dexterity} to the ability of dodging attacks)";

    public override void RevertEffect(EntityStats? entityStats)
    {
        entityStats?.ModifyEntityAttribute(PlayerAttributes.Dexterity, -1);
    }
    public override void ApplyEffect(EntityStats? entityStats)
    {
        entityStats?.ModifyEntityAttribute(PlayerAttributes.Dexterity, Dexterity);
    }
    public override void Use(AttackStrategy strategy, Entity? source, List<Entity>? target)
    {
        if (IsDisposed) return;

        if (source != null)
            source.EntityMoved += OnMoveHandler;
        ApplyEffect(source?.GetEntityStats());

        IsDisposed = true;
    }
    public override void Dispose(Entity? entity)
    {
        if (entity != null)
            entity.EntityMoved -= OnMoveHandler;
    }
    public override void OnMoveHandler(object sender, EventArgs e)
    {
        ActiveTime++;
        if (ActiveTime > Lifetime)
        {
            Dispose(sender as Entity);
            return;
        }
        RevertEffect((sender as Entity)?.GetEntityStats());
    }
    public override object Copy() => new DexterityPotion();
}

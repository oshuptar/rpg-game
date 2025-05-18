using RPG_Game.Entities;
using RPG_Game.Interfaces;
using RPG_Game.Entiities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Enums;
using RPG_Game.Controller;
using System.Text.Json.Serialization;
using RPG_Game.Model.Entities;

namespace RPG_Game.Potions;

// Implement via events in PlayerStats class

//Temporary potion
// They could be refilled again after some number of player moves
public class AggressionPotion : TemporaryPotion
{
    public override string Name => $"Aggresion Potion { (IsDisposed ? "(Disposed)" : "") }";
    [JsonInclude]
    public int Aggression => 10;
    protected override int ActiveTime { get; set; } = 0;
    public override int Lifetime => 10;
    public override void ApplyEffect(EntityStats? entityStats) 
    {
        entityStats?.ModifyEntityAttribute(PlayerAttributes.Aggression, Aggression);
    }
    public override void RevertEffect(EntityStats? entityStats) 
    {
        entityStats?.ModifyEntityAttribute(PlayerAttributes.Aggression, -Aggression);
    }
    public override string Description => $"(Adds {Aggression} Aggression)";
    public override void Use(AttackStrategy strategy, Entity? source, List<Entity>? target)
    {
        if (IsDisposed)
            return;

        if (source != null)
            source.EntityMoved += OnMoveHandler;
        ApplyEffect(source?.GetEntityStats());
        IsDisposed = true;
    }
    public override void Dispose(Entity? entity)
    {
        RevertEffect(entity?.GetEntityStats());
        if (entity != null)
            entity.EntityMoved -= OnMoveHandler;
    }
    public override void OnMoveHandler(object sender, EventArgs e)
    {
        ActiveTime++;
        if (ActiveTime > Lifetime)
            Dispose(sender as Entity);
    }
    public override object Copy() => new AggressionPotion();
}

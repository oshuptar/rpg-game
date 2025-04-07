using RPG_Game.Entities;
using RPG_Game.Interfaces;
using RPG_Game.Entiities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Enums;

namespace RPG_Game.Potions;

// Implement via events in PlayerStats class

//Temporary potion
// They could be refilled again after some number of player moves

// Add cloning to potions, since right now they all refer to the very same potion
public class AggressionPotion : TemporaryPotion
{
    public override string Name => $"Aggresion Potion { (IsDisposed ? "(Disposed)" : "") }";

    public int Aggression = 10;
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

    public override void Use(IEntity? entity)
    {
        if (IsDisposed)
            return;

        if (entity != null)
            entity.EntityMoved += OnMoveHandler;
        ApplyEffect(entity?.RetrieveEntityStats());
        IsDisposed = true;
    }

    public override void Dispose(IEntity? entity)
    {
        RevertEffect(entity?.RetrieveEntityStats());
        if (entity != null)
            entity.EntityMoved -= OnMoveHandler;
    }
    public override void OnMoveHandler(object sender, EventArgs e)
    {
        ActiveTime++;
        if (ActiveTime > Lifetime)
            Dispose(sender as IEntity);
    }

    public override object Copy() => new AggressionPotion();
}

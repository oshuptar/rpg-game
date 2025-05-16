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

// The ability of hero dodging attacks
public class DexterityPotion : TemporaryPotion
{
    public override string Name => $"Dexterity Potion {(IsDisposed ? "(Disposed)" : "")}";
    protected override int ActiveTime { get; set; } = 0;
    public override int Lifetime => 10;
    public int Dexterity = 10;
    public override string Description => $"(Adds {Dexterity} to the ability of dodging attacks)";

    public override void RevertEffect(EntityStats? entityStats)
    {
        entityStats?.ModifyEntityAttribute(PlayerAttributes.Dexterity, -1);
    }
    public override void ApplyEffect(EntityStats? entityStats)
    {
        entityStats?.ModifyEntityAttribute(PlayerAttributes.Dexterity, Dexterity);
    }
    public override void Use(AttackStrategy strategy, IEntity? source, List<IEntity>? target)
    {
        if (IsDisposed) return;

        if (source != null)
            source.EntityMoved += OnMoveHandler;
        ApplyEffect(source?.GetEntityStats());

        IsDisposed = true;
    }
    public override void Dispose(IEntity? entity)
    {
        if (entity != null)
            entity.EntityMoved -= OnMoveHandler;
    }
    public override void OnMoveHandler(object sender, EventArgs e)
    {
        ActiveTime++;
        if (ActiveTime > Lifetime)
        {
            Dispose(sender as IEntity);
            return;
        }
        RevertEffect((sender as IEntity)?.GetEntityStats());
    }
    public override object Copy() => new DexterityPotion();
}

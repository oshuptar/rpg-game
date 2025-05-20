using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.Model.Entities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$discriminator")]
[JsonDerivedType(typeof(Player), "Player")]
[JsonDerivedType(typeof(Orc), "Orc")]
[JsonDerivedType(typeof(Goblin), "Goblin")]
public abstract class Entity : IEntity
{
    [JsonInclude]
    public Position Position { get; protected set; }
    public event EventHandler? EntityDied;
    public event EventHandler? EntityMoved;
    public Entity() { }
    public abstract object Copy();
    public abstract EntityStats GetEntityStats();
    public abstract bool Move(Direction direction, AuthorityGameState room);
    public abstract void ReceiveDamage(int damage, Entity? source);
    public virtual void SetPosition(Position position)
    {
        Position = position;
    }

    // Pass info about the sender, like PlayerId
    protected void OnDeath(object sender, EventArgs e)
    {
        this.EntityDied?.Invoke(this, e);
    }
    protected virtual void OnMove()
    {
        EntityMoved?.Invoke(this, EventArgs.Empty);
    }
}

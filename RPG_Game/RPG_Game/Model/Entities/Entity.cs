using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    [JsonInclude]
    public EntityStats Stats { get; protected set; }
    [JsonInclude]
    public AttackType AttackType { get; set; } = AttackType.NormalAttack;
    [JsonInclude]
    public AttackStrategy AttackStrategy { get; set; } = new NormalAttackStrategy();

    public event EventHandler? EntityDied;

    public event EventHandler<MoveEventArgs>? EntityMoved;
    public Entity() { }
    public abstract object Copy();
    public virtual EntityStats GetEntityStats() => Stats;
    public virtual void SetPosition(Position position)
    {
        Position = position;
    }

    // Pass info about the sender, like PlayerId
    public abstract void Attack(Entity target);
    protected void OnDeath(object sender, EventArgs e)
    {
        this.EntityDied?.Invoke(this, e);
    }
    protected virtual void OnMove(Direction direction)
    {
        EntityMoved?.Invoke(this, new MoveEventArgs(direction));
    }

    public virtual Position GetNewPosition(Direction direction)
    {
        Position tempPos = Position;
        switch (direction)
        {
            case Direction.West:
                tempPos.X -= 1;
                break;
            case Direction.East:
                tempPos.X += 1;
                break;
            case Direction.North:
                tempPos.Y -= 1;
                break;
            case Direction.South:
                tempPos.Y += 1;
                break;
        }
        return tempPos;
    }
    public virtual Position? IsMovable(Direction direction, AuthorityGameState room) // whether we can move in the following direction
    {
        Position tempPos = GetNewPosition(direction);

        if (!room.IsPosAvailable(tempPos))
            return null;

        return tempPos;
    }

    public virtual bool Move(Direction direction, AuthorityGameState room)
    {

        Position? tempPos;
        if ((tempPos = IsMovable(direction, room)) != null)
        {
            room.RemoveEntity(this);
            room.AddEntity(this, tempPos.Value);
            OnMove(direction);
            return true;
        }
        return false;
    }
    public virtual void ReceiveDamage(int damage, Entity? source)
    {
        Stats.ModifyEntityAttribute(PlayerAttributes.Health, -damage);
        if (source != null)
        {
            Attack(source);
        }
    }
}

public class MoveEventArgs : EventArgs
{
    public Direction Direction { get; set; }
    public MoveEventArgs(Direction direction)
    {
        Direction = direction;
    }
}

using RPG_Game.Entities;
using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Model.Entities;

public abstract class Entity : IEntity
{
    public Position Position { get; protected set; }
    public event EventHandler? EntityDied;
    public event EventHandler? EntityMoved;
    public abstract object Copy();
    public abstract EntityStats GetEntityStats();
    public abstract bool Move(Direction direction, Room room);
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

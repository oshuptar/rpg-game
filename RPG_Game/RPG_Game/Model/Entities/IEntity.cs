using RPG_Game.Interfaces;
using RPG_Game.Model;
using RPG_Game.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public interface IEntity : ICanMove, ICanReceiveDamage, ICopyable
{
    public event EventHandler? EntityDied;
    public event EventHandler<MoveEventArgs>? EntityMoved;
    public EntityStats GetEntityStats();
}

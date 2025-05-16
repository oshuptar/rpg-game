using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public interface IEntity : ICanMove, ICanReceiveDamage, ICopyable
{
    public event EventHandler? OwnDeath;

    public event EventHandler? EntityMoved;
    public EntityStats GetEntityStats();
    public Position Position { get; set; }
}

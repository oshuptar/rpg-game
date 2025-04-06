using RPG_Game.Entiities;
using RPG_Game.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;

// Categorizing interface
public abstract class IPotion : IItem
{
    protected bool IsDisposed = false;
    public abstract void ApplyEffect(EntityStats? entityStats);
    public abstract void Dispose(IEntity? entity);
}

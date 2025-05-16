using RPG_Game.Controller;
using RPG_Game.Entiities;
using RPG_Game.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;

public abstract class IItem : ICopyable
{
    public abstract string Name { get; }
    public virtual int Capacity => 1;
    public virtual void ApplyChanges(EntityStats entityStats) { }
    public virtual void RevertChanges(EntityStats entityStats) { }
    public virtual void Use(AttackStrategy strategy, IEntity? source, List<IEntity>? target) { }
    public virtual void PickUp(IEntity entity) => ApplyChanges(entity.GetEntityStats());
    public virtual void Drop(IEntity entity) => RevertChanges(entity.GetEntityStats());
    public abstract object Copy();
    public abstract string Description { get; }

}

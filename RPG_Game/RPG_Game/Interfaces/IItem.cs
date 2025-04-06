using RPG_Game.Entiities;
using RPG_Game.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;

// Do items need to contain attributes of a Player?
public abstract class IItem
{
    public abstract string Name { get; }
    public virtual int Capacity => 1; // default capacity for every item
    public virtual void ApplyChanges(EntityStats entityStats) { }
    public virtual void RevertChanges(EntityStats entityStats) { }
    public virtual void Use(IEntity? entity = null) { }
    public virtual void PickUp(IEntity entity) => ApplyChanges(entity.RetrieveEntityStats());
    public virtual void Drop(IEntity entity) => RevertChanges(entity.RetrieveEntityStats());
    public abstract string Description { get; }

}

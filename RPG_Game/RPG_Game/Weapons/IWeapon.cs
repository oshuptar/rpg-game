using RPG_Game.Controller;
using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;

public abstract class IWeapon : IItem
{
    public abstract int Damage { get; }
    // THis field will be only accessed inside weapon classes, hence we can store it here
    //In case I need to provide Attack method for all IItem classes, I should move it there
    public abstract int RadiusOfAction { get; }
    public abstract void Use(AttackStrategy strategy, IEntity? source, List<IEntity>? target);
    public abstract void DispatchAttack(AttackStrategy strategy, IEntity? source, List<IEntity>? target, int damage);
}
public abstract class LightWeapon : IWeapon
{
    public override int Capacity => base.Capacity;
}

public abstract class HeavyWeapon : IWeapon
{
    public override int Capacity => 2;
}

public abstract class MagicWeapon : IWeapon
{ 
    public override int Capacity => base.Capacity;
}




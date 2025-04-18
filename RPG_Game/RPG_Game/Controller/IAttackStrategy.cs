using RPG_Game.Entities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public interface IAttackStrategy
{
    public int AttackRequestHandler(MagicWeapon magicWeapon, IEntity entity, int Damage);
    public int AttackRequestHandler(LightWeapon lightWeapon, IEntity entity, int Damage);
    public int AttackRequestHandler(HeavyWeapon heavyWeapon, IEntity entity, int Damage);

    public int DefenseRequestHandler(MagicWeapon magicWeapon, IEntity entity, int AttackValue);
    public int DefenseRequestHandler(HeavyWeapon heavyWeapon, IEntity entity, int AttackValue);
    public int DefenseRequestHandler(LightWeapon lightWeapon, IEntity entity, int AttackValue);
}

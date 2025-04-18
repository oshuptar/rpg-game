using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Weapons;

//two-handed weapon
public class Hammer : HeavyWeapon
{
    public override int Damage => 10;
    public override string Name => "Hammer";
    public override string Description => $"(Damage: {Damage}; Two-Handed)";
    public override int RadiusOfAction => 2;// Means that all enemies and players in the radius 1 from the hero would be affected
    public override void Use(AttackStrategy strategy, IEntity? source, List<IEntity>? target)
    {
        DispatchAttack(strategy, source, target, this.Damage);
    }
    public override void DispatchAttack(AttackStrategy strategy, IEntity? source, List<IEntity>? target, int damage)
    {
        if (target == null) return;
        foreach (var entity in target)
        {
            int DrawDamageValue = strategy.AttackRequestHandler(this, entity, damage);
            int DrawDefenseValue = strategy.DefenseRequestHandler(this, entity, DrawDamageValue);
            int ReceivedDamageValue = DrawDamageValue - DrawDefenseValue;
            entity.ReceiveDamage(ReceivedDamageValue);
        }
    }
    public override object Copy() => new Hammer();
}

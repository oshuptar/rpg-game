using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Weapons;

// Concrete Component
public class Sword : LightWeapon
{
    public override int Damage => 7;
    public override string Name => "Sword";
    public override string Description => $"(Damage: {Damage}; One-Handed)";
    public override int RadiusOfAction => 1;
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
            entity.ReceiveDamage(ReceivedDamageValue, source);
        }
    }
    public override object Copy() => new Sword();
}

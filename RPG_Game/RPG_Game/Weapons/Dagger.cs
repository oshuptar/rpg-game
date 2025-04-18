using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Weapons;

public class Dagger : LightWeapon
{
    public override int Damage => 5;
    public override string Name => "Dagger";
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
            int DrawDamageValue = strategy.AttackRequestHandler(this, entity, Damage);
            int DrawDefenseValue = strategy.DefenseRequestHandler(this, entity, DrawDamageValue);
            int ReceivedDamageValue = DrawDamageValue - DrawDefenseValue;
            entity.ReceiveDamage(ReceivedDamageValue);
        }
    }
    public override object Copy() => new Dagger();
}

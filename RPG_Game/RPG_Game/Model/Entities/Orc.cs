using RPG_Game.Controller;
using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.UIHandlers;
using RPG_Game.Weapons;
using RPG_Game.LogMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Model;
using RPG_Game.Model.Entities;
using System.Text.Json.Serialization;

namespace RPG_Game.Entities;

public class Orc : Entity
{
    [JsonInclude]
    public EnemyStats orcStats { get; private set; } = new EnemyStats();
    [JsonInclude]
    public Weapon Weapon { get; set; } = new Dagger();
    [JsonInclude]
    public AttackStrategy AttackStrategy { get; set; } = new NormalAttackStrategy();
    public override bool Move(Direction direction, Room room)
    {
        throw new NotImplementedException();
    }
    public void Attack(Entity target)
    {
        Weapon.Use(AttackStrategy, this, new List<Entity>() { target });
    }
    public override void ReceiveDamage(int damage, Entity? source)
    {
        orcStats.ModifyEntityAttribute(PlayerAttributes.Health, -damage);
        if (source != null)
        {
            //ClientConsoleView.GetInstance().LogMessage(new OnAttackMessage(source, this, damage));
            //ClientConsoleView.GetInstance().LogMessage(new OnEnemyDetectionMessage(this));
            Attack(source);
        }
    }
    public Orc() : base()
    {
        //this.orcStats.Died += OnDeath;
    }
    public override string ToString() => "Orc";
    public override EntityStats GetEntityStats() => this.orcStats;
    public override object Copy()
    {
        return new Orc();
    }
}

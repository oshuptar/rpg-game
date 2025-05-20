using RPG_Game.Controller;
using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.LogMessages;
using RPG_Game.Model;
using RPG_Game.Model.Entities;
using RPG_Game.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public class Goblin : Entity
{
    [JsonInclude]
    public EnemyStats goblinStats { get; private set; } = new EnemyStats();
    [JsonInclude]
    public AttackStrategy AttackStrategy { get; set; } = new NormalAttackStrategy();
    [JsonInclude]
    public Weapon Weapon { get; set; } = new Sword();
    public override bool Move(Direction direction, AuthorityGameState room)
    {
        throw new NotImplementedException();
    }
    public void Attack(Entity target)
    {
        Weapon.Use(AttackStrategy, this, new List<Entity>() { target });
    }
    public override void ReceiveDamage(int damage, Entity? source)
    {
        goblinStats.ModifyEntityAttribute(PlayerAttributes.Health, -damage);
        if (source != null)
        {
            //ClientConsoleView.GetInstance().LogMessage(new OnAttackMessage(source, this, damage));
            //ClientConsoleView.GetInstance().LogMessage(new OnEnemyDetectionMessage(this));
            Attack(source);
        }
    }
    public Goblin() 
    {
        //this.goblinStats.Died += OnDeath;
    }
    public override object Copy()
    {
        return new Goblin();
    }
    public override string ToString() => "Goblin";
    public override EntityStats GetEntityStats() => this.goblinStats;
}

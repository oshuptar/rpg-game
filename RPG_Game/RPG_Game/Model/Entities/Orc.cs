using RPG_Game.Controller;
using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
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
using RPG_Game.Controller.Server;

namespace RPG_Game.Entities;

public class Orc : Enemy
{
    [JsonInclude]
    public Weapon Weapon { get; set; } = new Dagger();
    public override void Attack(Entity target)
    {
        Weapon.Use(AttackStrategy, this, new List<Entity>() { target });
    }
    public Orc() : base()
    {
        this.Stats = new EnemyStats();
        EnemyStrategy = new CalmEnemyStrategy();
        //this.orcStats.Died += OnDeath;
    }
    public override string ToString() => "Orc";
    public override object Copy()
    {
        return new Orc();
    }
}
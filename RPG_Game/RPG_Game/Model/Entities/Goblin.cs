using RPG_Game.Controller;
using RPG_Game.Controller.Server;
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

public class Goblin : Enemy
{
    [JsonInclude]
    public Weapon Weapon { get; set; } = new Sword();
    public override void Attack(Entity target)
    {
        Weapon.Use(AttackStrategy, this, new List<Entity>() { target });
    }
    public Goblin() 
    {
        EnemyStrategy = new AggressiveEnemyStrategy();
        //this.goblinStats.Died += OnDeath;
    }
    public override object Copy()
    {
        return new Goblin();
    }
    public override string ToString() => "Goblin";
}

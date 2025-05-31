using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RPG_Game.Controller.Server;
using RPG_Game.Interfaces;

namespace RPG_Game.Model.Entities;

public abstract class Enemy : Entity
{
    [JsonIgnore]
    public IEnemyStrategy EnemyStrategy { get; set; } 

    public virtual void ExecuteEnemyStrategy(AuthorityGameState gameState)
    {
        EnemyStrategy.ExecuteStrategy(gameState, this);
    }
}

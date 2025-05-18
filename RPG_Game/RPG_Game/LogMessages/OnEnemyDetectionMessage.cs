using RPG_Game.Entities;
using RPG_Game.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.LogMessages;

public class OnEnemyDetectionMessage
{
    public Entity? Entity { get; set; }

    public OnEnemyDetectionMessage(Entity? entity)
    {
        this.Entity = entity;
    }
}

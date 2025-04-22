using RPG_Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.LogMessages;

public class OnEnemyDeathMessage
{
    public IEnemy enemy;

    public OnEnemyDeathMessage(IEnemy enemy)
    {
        this.enemy = enemy;
    }  
}

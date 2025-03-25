using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.LogMessages;

public class OnEnemyDetectionMessage
{
    public IEnemy? enemy { get; set; }

    public OnEnemyDetectionMessage(IEnemy? enemy)
    {
        this.enemy = enemy;
    }
}

using RPG_Game.Entities;
using RPG_Game.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.LogMessages;

public class OnAttackMessage
{
    public Entity source;
    public Entity target;
    public int Damage;
    public OnAttackMessage(Entity source, Entity target, int damage)
    {
        this.source = source;
        this.target = target;
        Damage = damage;
    }
}

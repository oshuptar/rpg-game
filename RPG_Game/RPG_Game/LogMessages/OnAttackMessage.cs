using RPG_Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.LogMessages;

public class OnAttackMessage
{
    public IEntity source;
    public IEntity target;
    public int Damage;
    public OnAttackMessage(IEntity source, IEntity target, int damage)
    {
        this.source = source;
        this.target = target;
        Damage = damage;
    }
}

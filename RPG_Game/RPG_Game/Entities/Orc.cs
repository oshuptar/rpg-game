using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public class Orc : IEnemy
{
    public bool Move(Direction direction, Room room)
    {
        throw new NotImplementedException();
    }

    public void ReceiveDamage(int damage)
    {
        throw new NotImplementedException();
    }

    public Orc() { }

    public Orc(Orc goblin)
    {
        // Deep copy
    }

    public IEnemy Clone()
    {
        return new Orc(this);
    }
}

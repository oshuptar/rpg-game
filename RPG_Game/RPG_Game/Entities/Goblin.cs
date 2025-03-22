using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public class Goblin : IEnemy
{
    public (int x, int y) Position { get; set; }
    public bool Move(Direction direction, Room room)
    {
        throw new NotImplementedException();
    }

    public void ReceiveDamage(int damage)
    {
        throw new NotImplementedException();
    }

    public Goblin(Goblin goblin)
    {
        // Deep copy
    }

    public Goblin() { }

    public IEnemy Clone()
    {
        return new Goblin(this);
    }

    public override string ToString() => "Goblin";
}

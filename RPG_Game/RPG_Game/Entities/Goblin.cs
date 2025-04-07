using RPG_Game.Entiities;
using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public class Goblin : IEnemy
{
    public (int x, int y) Position { get; set; }
    private EnemyStats goblinStats = new EnemyStats();

    public event EventHandler? EntityMoved;

    public bool Move(Direction direction, Room room)
    {
        throw new NotImplementedException();
    }

    public void ReceiveDamage(int damage)
    {
        throw new NotImplementedException();
    }
    public Goblin() { }

    public object Copy()
    {
        return new Goblin();
    }

    public override string ToString() => "Goblin";

    public EntityStats RetrieveEntityStats() => this.goblinStats;
}

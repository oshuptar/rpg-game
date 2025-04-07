using RPG_Game.Entiities;
using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public class Orc : IEnemy
{
    public (int x, int y) Position { get; set; }
    private EnemyStats orcStats = new EnemyStats();

    public event EventHandler? EntityMoved;

    public bool Move(Direction direction, Room room)
    {
        throw new NotImplementedException();
    }

    public void ReceiveDamage(int damage)
    {
        throw new NotImplementedException();
    }

    public Orc() { }

    public override string ToString() => "Orc";

    public EntityStats RetrieveEntityStats() => this.orcStats;

    public object Copy()
    {
        return new Orc();
    }
}

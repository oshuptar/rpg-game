using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;

public interface IEnemy : ICanMove, ICanReceiveDamage
{
    public IEnemy Clone();

    public (int x, int y) Position { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;

public interface ICanReceiveDamage
{
    public void ReceiveDamage(int damage);
}

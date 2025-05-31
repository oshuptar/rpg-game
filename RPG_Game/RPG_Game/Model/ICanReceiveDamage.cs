using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Model;
using RPG_Game.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;

public interface ICanReceiveDamage
{
    public void ReceiveDamage(int damage, Entity? source);
}

public interface ICanMove
{
    public bool Move(Direction direction, AuthorityGameState room);
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public interface ICanMove
{
    public bool Move(Direction direction, Room room);
}

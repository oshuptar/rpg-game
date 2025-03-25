using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.LogMessages;

public class OnMoveMessage
{
    public Direction direction { get; set; }
    public string Name { get; set; }
    public OnMoveMessage(Direction direction, string name)
    {
        this.direction = direction;
        Name = name;    
    }
}

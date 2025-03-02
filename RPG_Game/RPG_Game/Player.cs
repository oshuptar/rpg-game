using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public class Player
{
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Health { get; set; }
    public int Luck { get; set; }
    public int Aggresion { get; set; }

    private (int x, int y) Position;
    public Player()
    {
        Strength = 0;
        Dexterity = 0;
        Health = 0;
        Luck = 0;
        Aggresion = 0;
        Position = (1, 1);
    }

    public (int,int)? IsMovable(Direction direction, Room room)
    {
        (int x, int y) TempPos = this.Position;
        switch (direction)
        {
            case Direction.Left:
                TempPos.x -= 1;
                break;
            case Direction.Right:
                TempPos.x += 1;
                break;
            case Direction.Up:
                TempPos.y -= 1;
                break;
            case Direction.Down:
                TempPos.y += 1;
                break;
        }

        if (!room.IsPosAvailable(TempPos.x, TempPos.y))
            return null;

        return TempPos;
    }

    public bool PlayerMove(Direction direction, Room room)
    {
        if(IsMovable(direction, room) != null)
        {

        }
    }
}

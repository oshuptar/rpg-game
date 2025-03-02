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

    public (int, int) GetNewPosition(Direction direction)
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
        return TempPos;
    }

    public (int,int)? IsMovable(Direction direction, Room room) // whether we can move in the following direction
    {
        (int x, int y) TempPos = GetNewPosition(direction);

        if (!room.IsPosAvailable(TempPos.x, TempPos.y))
            return null;

        return TempPos;
    }

    public bool Move(Direction direction, Room room)
    {
        (int, int)? TempPos;
        if((TempPos = IsMovable(direction, room)) != null)
        {
            room.RemoveObject(CellType.Player, Position);
            room.AddObject(CellType.Player, ((int, int)) TempPos);
            this.Position = ((int, int))TempPos;
            return true;
        }
        return false;
    }
}

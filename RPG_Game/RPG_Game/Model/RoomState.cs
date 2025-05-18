using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Model;
public class RoomState
{
    public class Cell
    {
        public CellType CellType { get; set; } = CellType.Empty;
        public Entity? Entity { get; set; }
        public List<Item>? Items;
        public string PrintCell()
        {
            switch (CellType)
            {
                case CellType.Empty:
                    return " ";
                case CellType.Wall:
                    return "█";
                case CellType cType when (cType & CellType.Player) != 0:
                    Player? player = Entity as Player;
                    return player.PlayerId.ToString();
                case CellType cType when (cType & CellType.Enemy) != 0:
                    return "E";
                default:
                    return "I";
            }
        }
        public bool IsWalkable()
        {
            if ((CellType & CellType.Wall) != 0
                || (CellType & CellType.Enemy) != 0
                || (CellType & CellType.Player) != 0) return false;
            return true;
        }
        public bool IsWall()
        {
            if (CellType == CellType.Wall)
                return true;
            return false;
        }
    };
    public Dictionary<int, Player> ActivePlayers = new Dictionary<int, Player>();
    public List<Entity> Entities = new List<Entity>();
    public Cell[,] Grid = new Cell[MapSettings.Width, MapSettings.Height];

    public RoomState()
    {
        for (int i = 0; i < MapSettings.Width; i++)
            for (int j = 0; j < MapSettings.Height; j++)
                Grid[i, j] = new Cell();
    }
    public StringBuilder RenderMap()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < MapSettings.Height; i++)
        {
            for (int j = 0; j < MapSettings.Width; j++)
            {
                sb.Append(Grid[j, i].PrintCell());
            }
            sb.Append('\n');
        }
        return sb;
    }
}

public class Position
{
    public int X;
    public int Y;
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
    public Position((int x, int y) position)
    {
        X = position.x;
        Y = position.y;
    }
}

public class MapSettings
{
    public const int DefaultWidth = 41;
    public const int DefaultHeight = 21;
    public const int FrameSize = 1;
    public const int Width = DefaultWidth + 2 * FrameSize;
    public const int Height = DefaultWidth + 2 * FrameSize;
}

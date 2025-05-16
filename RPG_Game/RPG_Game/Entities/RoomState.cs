using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public class RoomState
{
    public class Cell
    {
        public CellType CellType { get; set; } = CellType.Empty;
        public IEntity? Entity { get; set; } // Two entities cannot be on the same field at the same time

        public List<IItem>? Items;
        public string PrintCell()
        {
            switch (CellType)
            {
                case CellType.Empty:
                    return " ";
                case CellType.Wall:
                    return "█";
                case CellType cType when (cType & CellType.Player) != 0:
                    return "¶";
                case CellType cType when (cType & CellType.Enemy) != 0:
                    return "E";
                default:
                    return "I";
            }
        }
        public bool IsWalkable()
        {
            if ((CellType & CellType.Wall) != 0
                || (CellType & CellType.Enemy) != 0) return false;
            return true;
        }
        public bool IsWall()
        {
            if (CellType == CellType.Wall)
                return true;
            return false;
        }
    };
    private Cell[,] Grid = new Cell[MapSettings.Width, MapSettings.Height];
    public List<IEnemy> Enemies = new List<IEnemy>();
    public Cell[,] GetGrid() => Grid;
}

public class MapSettings
{
    public const int DefaultWidth = 41;
    public const int DefaultHeight = 21;
    public const int FrameSize = 1;
    public const int Width = DefaultWidth + 2 * FrameSize;
    public const int Height = DefaultWidth + 2 * FrameSize;
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

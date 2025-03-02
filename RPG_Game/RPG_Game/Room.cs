using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public class Room
{
    //private class ensures encapsulation
    private class Cell // do we need to use decorator pattern for a cell?
    {
        public CellType CellType { get; set; } = CellType.Empty;


        public void PrintCell()
        {
            switch(CellType)
            {
                case CellType.Empty:
                    Console.Write(" ");
                    break;
                case CellType.Wall:
                    Console.Write("█");
                    break;
                case CellType.Player:
                    Console.Write("¶");
                    break;
                default:
                    Console.Write("I");
                    break;

            }
        }

        public bool IsWalkable()
        {
            if (CellType == CellType.Wall || CellType == CellType.Player) return false;
            return true;
        }
    };

    private const int _width = 22; // additional 2 accounts for a wall as an outer frame
    private const int _height = 42; // additional 2 accounts for a wall as an outer frame
    private Cell[,] Grid { get; } = new Cell[_height, _width]; //Array of references

    public Room()
    {
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                Grid[i, j] = new Cell();
                if (i == 0 || j == 0 || i == _height - 1 || j == _width - 1)
                    Grid[i, j].CellType = CellType.Wall;
            }
        }
        Grid[1, 1].CellType = CellType.Player;
    }

    public void PrintGrid()
    {
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                Grid[i, j].PrintCell();
            }
            Console.WriteLine();
        }

    }

    public bool IsPosAvailable(int x, int y) => Grid[x, y].IsWalkable();

    public bool RemoveObject(CellType cellType, (int x, int y) position) //assuming the position is the position of the player
    {
        if (Grid[position.x, position.y].CellType != cellType)
            return false;
        Grid[position.x, position.y].CellType = Grid[position.x, position.y].CellType & ~cellType;
        return true;
    }

    public bool AddObject(CellType cellType, (int x, int y) position) //assuming the position is the position of the player; this ensures that we do not need to check bounds
    {
        if (Grid[position.x, position.y].CellType != cellType) // we assume only one object of a type can be placed at the position; at least for now
        {

        }
    }
}

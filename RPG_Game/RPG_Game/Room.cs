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
    private const int _defaultWidth = 20;
    private const int _defaultHeight = 40;
    private const int _frameSize = 1;
    private const int _width = _defaultWidth + 2*_frameSize; // additional 2 accounts for a wall as an outer frame
    private const int _height = _defaultHeight + 2*_frameSize; // additional 2 accounts for a wall as an outer frame
    private Cell[,] Grid { get; } = new Cell[_width, _height]; //Array of references
    public List<IItem>[,] Items { get; } = new List<IItem>[_width, _height]; // Will be used to store items on each of the tile of the room

    public Room()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                Grid[i, j] = new Cell();
                if (i == 0 || j == 0 || i == _width - 1 || j == _height - 1)
                    Grid[i, j].CellType = CellType.Wall;
            }
        }
        Grid[1, 1].CellType = CellType.Player;

        //Let's say we would have 10% of obstacles of the total map size
        int widthPlayAreaSize = (_width - 2 * _frameSize);
        int heightPlayAreaSize = (_height - 2 * _frameSize);
        Random random = new Random();
        for(int i = 0; i < widthPlayAreaSize*heightPlayAreaSize/10; i++)
        {
            int X = _frameSize + random.Next() % (widthPlayAreaSize - _frameSize);
            int Y = _frameSize + random.Next() % (heightPlayAreaSize - _frameSize);
            AddObject(CellType.Wall, (X, Y));
        }
    }

    public void PrintGrid()
    {
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                Grid[j, i].PrintCell();
            }
            Console.WriteLine();
        }

    }

    public bool IsPosAvailable(int x, int y)
    {
        if(x < 0 || y < 0 || x > _width - 1 || y > _height - 1)
            return false;
        return Grid[x, y].IsWalkable();
    }

    public bool RemoveObject(CellType cellType, (int x, int y) position) //assuming the position is the position of the player
    {
        if ((Grid[position.x, position.y].CellType & cellType) == 0)
            return false;
        //if (Items[position.x, position.y].Count == 0)
            Grid[position.x, position.y].CellType = Grid[position.x, position.y].CellType & ~cellType;
        return true;
    }

    public bool AddObject(CellType cellType, (int x, int y) position) //assuming the position is the position of the player; this ensures that we do not need to check bounds
    {
        if ((Grid[position.x, position.y].CellType & cellType) != 0) // we assume only one object of a type can be placed at the position; at least for now
            return false;

        // no need to check for a wall
        Grid[position.x, position.y].CellType = Grid[position.x, position.y].CellType | cellType;
        return true;
    }

    public void DisplayTileItems((int x, int y) position)
    {
        string output = String.Join(',', Items[position.x, position.y]);
        Console.WriteLine($"Items: {output}");
    }

    //public IItem ChooseItem((int x, int y) position)
    //{

    //}
}

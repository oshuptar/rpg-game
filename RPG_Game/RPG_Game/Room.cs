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
            switch (this.CellType)
            {
                case CellType.Empty:
                    Console.Write(" ");
                    break;
                case CellType.Wall:
                    Console.Write("█");
                    break;
                //Ensures Player sign stays on top
                case CellType cType when ((cType & CellType.Player) != 0): // Constant pattern matching
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


    public const int _defaultWidth = 20;
    public const int _defaultHeight = 40;
    public const int _frameSize = 1;
    public const int _width = _defaultWidth + 2 * _frameSize; // additional 2 accounts for a wall as an outer frame
    public const int _height = _defaultHeight + 2 * _frameSize; 

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

        RoomGenerator.RandomRoomGeneration(this);
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
        if (x < 0 || y < 0 || x > _width - 1 || y > _height - 1)
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

    //assuming the position is the position of the player; this ensures that we do not need to check bounds
    // this method is called after the check or from active position
    public bool AddObject(CellType cellType, (int x, int y) position) 
    {
        Grid[position.x, position.y].CellType = Grid[position.x, position.y].CellType | cellType;
        return true;
    }

    public void AddItem(IItem item, (int x, int y) position)
    {
        if (!this.Grid[position.x, position.y].IsWalkable())
            return;

        AddObject(CellType.Item, position);

        if (this.Items[position.x, position.y] == null)
            this.Items[position.x, position.y] = new List<IItem>()
                ;
        this.Items[position.x, position.y].Add(item);
    }

    public void DisplayTileItems((int x, int y) position)
    {
        string? output = null;
        if (Items[position.x, position.y] != null)
        {
            // On each element of the sequence the lambda function is called
            output = String.Join(',', Items[position.x, position.y].Select(item => item.Name)); 
        }
        // Displays none if output == null
        Console.WriteLine($"Items: {output ?? "None"}"); 
    }

    //public IItem ChooseItem((int x, int y) position)
    //{

    //}
}


//if ((Grid[position.x, position.y].CellType & cellType) != 0) // we assume only one object of a type can be placed at the position; at least for now
//    return false;

// no need to check for a wall
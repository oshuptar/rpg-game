using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public class Room
{
    //private class ensures encapsulation
    public class Cell // do we need to use decorator pattern for a cell?
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
    public Cell[,] Grid { get; } = new Cell[_width, _height]; //Array of references
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

    public bool IsPosAvailable(int x, int y)
    {
        if (x < 0 || y < 0 || x > _width - 1 || y > _height - 1)
            return false;
        return Grid[x, y].IsWalkable();
    }

    //assuming the position is the position of the player; this ensures that we do not need to check bounds
    // this method is called after the check or from active position
    public bool AddObject(CellType cellType, (int x, int y) position)
    {
        Grid[position.x, position.y].CellType = Grid[position.x, position.y].CellType | cellType;
        return true;
    }

    public bool RemoveObject(CellType cellType, (int x, int y) position) //assuming the position is the position of the player
    {
        if ((Grid[position.x, position.y].CellType & cellType) == 0)
            return false;
        // This if-statement needs a better handling
        if (Items[position.x, position.y]?.Count == 0 || (cellType & CellType.Item) == 0)
            Grid[position.x, position.y].CellType = Grid[position.x, position.y].CellType & ~cellType;

        return true;
    }

    public void AddItem(IItem item, (int x, int y) position)
    {
        if (!this.IsPosAvailable(position.x, position.y))
            return;

        AddObject(CellType.Item, position);

        if (this.Items[position.x, position.y] == null)
            this.Items[position.x, position.y] = new List<IItem>();
                
        this.Items[position.x, position.y].Add(item);
    }

    // index denotes the index of the item from the list to be removed
    public IItem? RemoveItem((int x, int y) position, int index = 0)
    {
        if (this.Items[position.x, position.y] == null || this.Items[position.x, position.y].Count == 0)
            return null;

        IItem tempItem = this.Items[position.x, position.y].ElementAt(index);
        this.Items[position.x, position.y].RemoveAt(index);
        this.RemoveObject(CellType.Item, position);

        return tempItem;
    }

    //public IItem ChooseItem((int x, int y) position)
    //{

    //}
}


//if ((Grid[position.x, position.y].CellType & cellType) != 0) // we assume only one object of a type can be placed at the position; at least for now
//    return false;

// no need to check for a wall
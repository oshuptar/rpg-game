using RPG_Game.Enums;
using RPG_Game.HelperClasses;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entiities;

public class Room
{
    public class Cell 
    {
        public CellType CellType { get; set; } = CellType.Empty;

        public IEnemy? Enemy { get; set; }

        public List<IItem>? Items;
        public string PrintCell()
        {
            switch (CellType)
            {
                case CellType.Empty:
                    return " ";
                case CellType.Wall:
                    return "█";
                case CellType cType when (cType & CellType.Player) != 0: // Constant pattern matching
                    return "¶";
                case CellType cType when (cType & CellType.Enemy) != 0:
                    return "E";
                default:
                    return "I";
            }
        }
        public bool IsWalkable()
        {
            if (CellType == CellType.Wall) return false;
            return true;
        }
    };

    public const int _defaultWidth = 41;
    public const int _defaultHeight = 21;
    public const int _frameSize = 1;
    public const int _width = _defaultWidth + 2 * _frameSize; // additional 2 accounts for a wall as an outer frame
    public const int _height = _defaultHeight + 2 * _frameSize; 
    private Cell[,] Grid = new Cell[_width, _height]; //Array of references

    public Cell[,] RetrieveGrid() => Grid;
    public Room() { }
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
        Grid[position.x, position.y].CellType |= cellType;
        return true;
    }

    public void AddEnemy(IEnemy enemy, (int x, int y) position)
    {
        if (!IsPosAvailable(position.x, position.y) || Grid[position.x, position.y].CellType == CellType.Player)
            return;

        if (Grid[position.x, position.y].Enemy != null)
            return;

        AddObject(CellType.Enemy, (position.x , position.y));
        Grid[position.x, position.y].Enemy = enemy.Clone(); // this ensures a deep copy of enemy attributes
    }

    public bool RemoveObject(CellType cellType, (int x, int y) position) //assuming the position is the position of the player
    {
        if ((Grid[position.x, position.y].CellType & cellType) == 0)
            return false;
        // This if-statement needs a better handling
        if (Grid[position.x, position.y].Items?.Count == 0 || (cellType & CellType.Item) == 0)
            Grid[position.x, position.y].CellType &= ~cellType;

        return true;
    }
    public void AddItem(IItem? item, (int x, int y) position)
    {
        if (!IsPosAvailable(position.x, position.y) || item == null)
            return;

        AddObject(CellType.Item, position);

        if (Grid[position.x, position.y].Items == null)
            Grid[position.x, position.y].Items = new List<IItem>();

        Grid[position.x, position.y].Items!.Add(item);
    }

    // index denotes the index of the item from the list to be removed
    public IItem? RemoveItem((int x, int y) position, int index = 0)
    {
        if (Grid[position.x, position.y].Items == null || Grid[position.x, position.y].Items?.Count == 0)
            return null;

        IItem tempItem = Grid[position.x, position.y].Items!.ElementAt(index);
        Grid[position.x, position.y].Items!.RemoveAt(index);
        RemoveObject(CellType.Item, position);

        return tempItem;
    }
}

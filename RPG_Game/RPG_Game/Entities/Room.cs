using RPG_Game.Entities;
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

    public const int _defaultWidth = 41;
    public const int _defaultHeight = 21;
    public const int _frameSize = 1;
    public const int _width = _defaultWidth + 2 * _frameSize; // additional 2 accounts for a wall as an outer frame
    public const int _height = _defaultHeight + 2 * _frameSize; 
    private Cell[,] Grid = new Cell[_width, _height]; //Array of references

    public List<IEnemy> Enemies = new List<IEnemy>();  // for the sake of calculatingthe distance to the closest enemy
    public Cell[,] RetrieveGrid() => Grid;
    public Room() { }
    public bool IsPosAvailable(int x, int y)
    {
        if (x < 0 || y < 0 || x > _width - _frameSize || y > _height - _frameSize)
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

        if (Grid[position.x, position.y].Entity != null)
            return;

        AddObject(CellType.Enemy, (position.x , position.y));
        enemy.Position = (position.x, position.y);
        Enemies.Add(enemy);
        Grid[position.x, position.y].Entity = enemy; // this ensures a deep copy of enemy attributes
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

    public void RemoveEntity(IEnemy enemy)
    {
        IEntity? entity = Grid[enemy.Position.x, enemy.Position.y].Entity;
        if (entity != null && entity.Equals(enemy))
        {
            Grid[enemy.Position.x, enemy.Position.y].Entity = null;
            this.RemoveObject(CellType.Enemy, (enemy.Position.x, enemy.Position.y));
            this.Enemies.Remove(enemy);
        }
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
    public bool IsInRange((int x, int y) position)
    {
        if (position.x >= _frameSize && position.x < _width - _frameSize
            && position.y >= _frameSize && position.y < _height - _frameSize)
            return true;
        return false;
    }

    // BFS will be used to retrieve enemies around the player
    public void BFS(List<IEntity> entities, bool[,] visited, List<(int x, int y)> directions,
        int maxDepth, Queue<(Position position, int depth)> bfsQueue)
    {
        while (bfsQueue.Count > 0)
        {
            (Position positon, int depth) = bfsQueue.Dequeue();
            if (depth > maxDepth)
                continue;

            IEntity? entity = Grid[positon.X, positon.Y].Entity;
            if (entity != null)
                entities.Add(entity);
            foreach (var direction in directions)
            {
                (int X, int Y) newPosition = (positon.X + direction.x, positon.Y + direction.y);
                if (IsInRange(newPosition) && !Grid[newPosition.X, newPosition.Y].IsWall() && !visited[newPosition.X, newPosition.Y])
                {
                    bfsQueue.Enqueue((new Position(newPosition), depth + 1));
                    visited[newPosition.X, newPosition.Y] = true;
                }
            }
        }
    }

    public List<IEntity>? RetrieveEnemiesInRadius(IEntity entity, int radius)
    {
        List<(int x, int y)> directions = new List<(int, int)> { (-1, 0), (1, 0), (0, 1), (0, -1) };
        List<IEntity> entities = new List<IEntity>();
        bool[,] visited = new bool[_width + 1, _height + 1];

        for (int i = 0; i < _width; i++)
            for (int j = 0; j < _height; j++)
                visited[i, j] = false;

        Queue<(Position, int)> bfsQueue = new Queue<(Position, int)>();
        bfsQueue.Enqueue((new Position(entity.Position), 0));
        BFS(entities, visited, directions, radius, bfsQueue);
        return entities;
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


//first Dispatch to the weapon -> weapon type is known
//Second dispatch to the attack object which knows how to compute the damage for the received weapon
//Third dispatch to the enemy (player) which receives the damage, knowing the attack type and weapon type

//weapon attack and defense methods

// so attack visitor
// and defend visitor

// attack is performed -> weapon is known
// the attack type is not known
// dipatch goes to the attack type class, which is visited
//calculates attack value

// then the attack value is dispatched to the player(entity) with the weapon used

// the Defense method in item is triggered, passing the player attributes and the weapon used
// the weapon dispatched it to the Attack Type used
// the defense is calculated and returned to the hero

// the hero subtracts the damage receives - teh defense value and changes its attributes

// Question now - how to remember the attack type?

// Conclusion, we sould pass attack type as well
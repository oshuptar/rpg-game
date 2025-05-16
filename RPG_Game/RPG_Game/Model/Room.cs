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
    private RoomState _roomState;
    public Room()
    {
        _roomState = new RoomState();
    }
    public bool IsPosAvailable(Position position)
    {
        if (position.X < 0 || position.Y < 0 
            || position.X > MapSettings.Width - MapSettings.FrameSize 
            || position.Y > MapSettings.Height - MapSettings.FrameSize)
            return false;
        return _roomState.GetGrid()[position.X, position.Y].IsWalkable();
    }
    public bool AddObject(CellType cellType, Position position)
    {
        _roomState.GetGrid()[position.X, position.Y].CellType |= cellType;
        return true;
    }
    public void AddEnemy(IEnemy enemy, Position position)
    {
        if (!IsPosAvailable(position) || _roomState.GetGrid()[position.X, position.Y].CellType == CellType.Player)
            return;

        if (_roomState.GetGrid()[position.X, position.Y].Entity != null)
            return;

        AddObject(CellType.Enemy, position);
        enemy.Position = position;
        _roomState.Enemies.Add(enemy);
        _roomState.GetGrid()[position.X, position.Y].Entity = enemy; // this ensures a deep copy of enemy attributes
    }
    public bool RemoveObject(CellType cellType, Position position) //assuming the position is the position of the player
    {
        if ((_roomState.GetGrid()[position.X, position.Y].CellType & cellType) == 0)
            return false;
        if (_roomState.GetGrid()[position.X, position.Y].Items?.Count == 0 || (cellType & CellType.Item) == 0)
            _roomState.GetGrid()[position.X, position.Y].CellType &= ~cellType;

        return true;
    }
    public void AddItem(IItem? item, Position position)
    {
        if (!IsPosAvailable(position) || item == null)
            return;

        AddObject(CellType.Item, position);

        if (_roomState.GetGrid()[position.X, position.Y].Items == null)
            _roomState.GetGrid()[position.X, position.Y].Items = new List<IItem>();

        _roomState.GetGrid()[position.X, position.Y].Items!.Add(item);
    }
    public void RemoveEntity(IEnemy enemy)
    {
        IEntity? entity = _roomState.GetGrid()[enemy.Position.X, enemy.Position.Y].Entity;
        if (entity != null && entity.Equals(enemy))
        {
            _roomState.GetGrid()[enemy.Position.X, enemy.Position.Y].Entity = null;
            this.RemoveObject(CellType.Enemy, enemy.Position);
            _roomState.Enemies.Remove(enemy);
        }
    }
    // index denotes the index of the item from the list to be removed
    public IItem? RemoveItem(Position position, int index = 0)
    {
        if (_roomState.GetGrid()[position.X, position.Y].Items == null 
            || _roomState.GetGrid()[position.X, position.Y].Items?.Count == 0)
            return null;

        IItem tempItem = _roomState.GetGrid()[position.X, position.Y].Items!.ElementAt(index);
        _roomState.GetGrid()[position.X, position.Y].Items!.RemoveAt(index);
        RemoveObject(CellType.Item, position);

        return tempItem;
    }
    public bool IsInRange(Position position)
    {
        if (position.X >= MapSettings.FrameSize && position.X < MapSettings.Width - MapSettings.FrameSize
            && position.Y >= MapSettings.FrameSize && position.Y < MapSettings.Height - MapSettings.FrameSize)
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

            IEntity? entity = _roomState.GetGrid()[positon.X, positon.Y].Entity;
            if (entity != null)
                entities.Add(entity);
            foreach (var direction in directions)
            {
                Position newPosition = new Position(positon.X + direction.x, positon.Y + direction.y);
                if (IsInRange(newPosition) && !_roomState.GetGrid()[newPosition.X, newPosition.Y].IsWall() && !visited[newPosition.X, newPosition.Y])
                {
                    bfsQueue.Enqueue((newPosition, depth + 1));
                    visited[newPosition.X, newPosition.Y] = true;
                }
            }
        }
    }
    public List<IEntity>? RetrieveEnemiesInRadius(IEntity entity, int radius)
    {
        List<(int x, int y)> directions = new List<(int, int)> { (-1, 0), (1, 0), (0, 1), (0, -1) };
        List<IEntity> entities = new List<IEntity>();
        bool[,] visited = new bool[MapSettings.Width + 1, MapSettings.Height + 1];

        for (int i = 0; i < MapSettings.Width; i++)
            for (int j = 0; j < MapSettings.Height; j++)
                visited[i, j] = false;

        Queue<(Position, int)> bfsQueue = new Queue<(Position, int)>();
        bfsQueue.Enqueue((new Position(entity.Position.X, entity.Position.Y), 0));
        BFS(entities, visited, directions, radius, bfsQueue);
        return entities;
    }
    public RoomState GetRoomState() => _roomState;
}


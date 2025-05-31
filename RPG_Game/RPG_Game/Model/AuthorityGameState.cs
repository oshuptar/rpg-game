using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.HelperClasses;
using RPG_Game.Interfaces;
using RPG_Game.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Model;

public class AuthorityGameState : IGameState
{
    private RoomState RoomState { get; set; }
    public int PlayerId { get; set; }
    public AuthorityGameState() { }
    public AuthorityGameState(RoomState roomState)
    {
        RoomState = roomState;
    }
    public AuthorityGameState(RoomState roomState, int playerID)
    {
        RoomState = roomState;
        PlayerId = playerID;
    }
    public bool AddObject(CellType cellType, Position position)
    {
        RoomState.LockWriteBlock(position);
        try
        {
            RoomState.Grid[position.X][position.Y].CellType |= cellType;
            return true;
        }
        finally { RoomState.UnlockWriteBlock(position); }
    }
    public bool RemoveObject(CellType cellType, Position position)
    {
        RoomState.LockWriteBlock(position);
        try
        {
            if ((RoomState.Grid[position.X][position.Y].CellType & cellType) == 0)
                return false;

            if (RoomState.Grid[position.X][position.Y].Items?.Count == 0 || (cellType & CellType.Item) == 0)
                RoomState.Grid[position.X][position.Y].CellType &= ~cellType;

            return true;
        }
        finally { RoomState.UnlockWriteBlock(position); }
    }
    public bool AddEntity(Entity entity, Position position)
    {
        RoomState.LockWriteBlock(position);
        try
        {
            if (!IsPosAvailable(position))
                return false;

            if (RoomState.Grid[position.X][position.Y].Entity != null)
                return false;

            AddObject(CellType.Enemy, position);
            entity.SetPosition(position);

            try
            {
                RoomState.StateLock.EnterWriteLock();
                RoomState.Entities.Add(entity);
            }
            finally { RoomState.StateLock.ExitWriteLock(); }

            RoomState.Grid[position.X][position.Y].Entity = entity;
            return true;
        }
        finally { RoomState.UnlockWriteBlock(position); }
    }
    public void RemoveEntity(Entity entity)
    {
        RoomState.LockWriteBlock(entity.Position);
        try
        {
            Entity? removedEntity = RoomState.Grid[entity.Position.X][entity.Position.Y].Entity;
            if (removedEntity != null)
            {
                RoomState.Grid[entity.Position.X][entity.Position.Y].Entity = null;
                RemoveObject(CellType.Enemy, entity.Position);

                try
                {
                    RoomState.StateLock.EnterWriteLock();
                    RoomState.Entities.Remove(entity);
                }
                finally { RoomState.StateLock.ExitWriteLock(); }
            }
        }
        finally { RoomState.UnlockWriteBlock(entity.Position); }
    }
    public bool AddPlayer(Player player, Position position)
    {
        RoomState.LockWriteBlock(position);
        try
        {
            if (!IsPosAvailable(position))
                return false;

            if (RoomState.Grid[position.X][position.Y].Entity != null)
                return false;

            AddObject(CellType.Player, position);
            player.SetPosition(position);
            RoomState.ActivePlayers.TryAdd(player.PlayerId, player);
            RoomState.Grid[position.X][position.Y].Entity = player;
            return true;

        }
        finally { RoomState.UnlockWriteBlock(position); }
    }
    public void RemovePlayer(Player player)
    {
        RoomState.LockWriteBlock(player.Position);
        try
        {
            RoomState.ActivePlayers.TryGetValue(player.PlayerId, out Player? removedPlayer);
            if (removedPlayer != null /*&& removedPlayer.Equals(player)*/)
            {
                RoomState.Grid[player.Position.X][player.Position.Y].Entity = null;
                //RemoveObject(CellType.Player, player.Position);
                RoomState.Grid[player.Position.X][player.Position.Y].CellType &= ~CellType.Player;
                RoomState.ActivePlayers.TryRemove(player.PlayerId, out var player1);
            }
        }
        finally { RoomState.UnlockWriteBlock(player.Position); }
    }
    public void AddItem(Item? item, Position position)
    {
        RoomState.LockWriteBlock(position);
        try
        { 
            if (RoomState.Grid[position.X][position.Y].IsWall() || item == null)
                return;

            AddObject(CellType.Item, position);

            if (RoomState.Grid[position.X][position.Y].Items == null)
                RoomState.Grid[position.X][position.Y].Items = new List<Item>();

            RoomState.Grid[position.X][position.Y].Items!.Add(item);
        }
        finally { RoomState.UnlockWriteBlock(position); }
    }
    public Item? RemoveItem(Position position, int index = 0)
    {
        RoomState.LockWriteBlock(position);
        try
        {
            if (RoomState.Grid[position.X][position.Y].Items == null
                || RoomState.Grid[position.X][position.Y].Items?.Count == 0)
                return null;

            Item tempItem = RoomState.Grid[position.X][position.Y].Items!.ElementAt(index);
            RoomState.Grid[position.X][position.Y].Items!.RemoveAt(index);
            RemoveObject(CellType.Item, position);
            return tempItem;
        }
        finally { RoomState.UnlockWriteBlock(position); }
    }
    public Item? RemoveItem(Position position, Item item)
    {
        try
        {
            RoomState.LockWriteBlock(position);

            var items = RoomState.Grid[position.X][position.Y].Items;
            if (items == null
                || items?.Count == 0)
                return null;

            Item? tempItem = items?.FirstOrDefault(i => i.Equals(item));
            if (tempItem == null) return null;

            items!.Remove(tempItem);
            RemoveObject(CellType.Item, position);

            return tempItem;
        }
        finally { RoomState.UnlockWriteBlock(position); }
    }
    public bool IsPosAvailable(Position position)
    {
        try
        {
            RoomState.LockReadBlock(position);
            if (!IsInRange(position))
                return false;
            return RoomState.Grid[position.X][position.Y].IsWalkable();
        }
        finally { RoomState.UnlockReadBlock(position); }
    }
    public bool IsInRange(Position position)
    {
        return MapSettings.IsInRange(position);
    }

    // Note that no Copy was done, hence the references are dynamic
    // Introduce locking mechanism on printing
    public GameState GetGameState()
    {
        return new GameState(RoomState, PlayerId);
    }
    //public GameState GetGameState()
    public RoomState GetRoomState() => RoomState;

    public Player GetPlayer()
    {
        return RoomState.ActivePlayers[PlayerId];
    }
    public List<Entity> GetVisibleEnemies()
    {
        try
        {
            RoomState.StateLock.EnterReadLock();
            return RoomState.Entities;
        }
        finally { RoomState.StateLock.ExitReadLock(); }
    }
    public List<Entity> GetVisibleEntities()
    {
        try
        {
            RoomState.StateLock.EnterReadLock();
            return RoomState.Entities.Concat(RoomState.ActivePlayers.Values
                .Where((player) => player.PlayerId != PlayerId))
                .ToList();
        }
        finally { RoomState.StateLock.ExitWriteLock(); }
    }
    public List<Item>? GetItems(Position pos)
    {
        try
        {
            RoomState.LockReadBlock(pos);
            return RoomState.Grid[pos.X][pos.Y].Items;
        }
        finally { RoomState.UnlockReadBlock(pos); }
    }
    public StringBuilder RenderMap()
    {
        return RoomState.RenderMap();
    }
    public void LockReadBlock(Position position)
        => RoomState.LockReadBlock(position);
    public void LockWriteBlock(Position position)
        => RoomState.LockWriteBlock(position);
    public void UnlockReadBlock(Position position)
        => RoomState.UnlockReadBlock(position);
    public void UnlockWriteBlock(Position position)
        => RoomState.UnlockWriteBlock(position);
    public void LockReadState()
        => RoomState.StateLock.EnterReadLock();
    public void LockWriteState()
        => RoomState.StateLock.EnterWriteLock();
    public void UnlockReadState()
       => RoomState.StateLock.ExitReadLock();
    public void UnlockWriteState()
        => RoomState.StateLock.ExitWriteLock();
}


//private void BFS(List<(Entity, int)> entities, bool[,] visited, List<(int x, int y)> directions,
//    int maxDepth, Queue<(Position position, int depth)> bfsQueue, Entity? source)
//{
//    while (bfsQueue.Count > 0)
//    {
//        (Position positon, int depth) = bfsQueue.Dequeue();
//        if (depth > maxDepth)
//            continue;

//        Entity? entity = RoomState.Grid[positon.X][positon.Y].Entity;
//        if (entity != null && !entity.Equals(source))
//            entities.Add((entity, depth));
//        foreach (var direction in directions)
//        {
//            Position newPosition = new Position(positon.X + direction.x, positon.Y + direction.y);
//            if (IsInRange(newPosition) && !RoomState.Grid[newPosition.X][newPosition.Y].IsWall() && !visited[newPosition.X, newPosition.Y])
//            {
//                bfsQueue.Enqueue((newPosition, depth + 1));
//                visited[newPosition.X, newPosition.Y] = true;
//            }
//        }
//    }
//}
//public List<(Entity, int)> RetrieveEntitiesInRadius(Entity entity, int radius)
//{
//    List<(int x, int y)> directions = new List<(int, int)> { (-1, 0), (1, 0), (0, 1), (0, -1) };
//    List<(Entity, int)> entities = new List<(Entity, int)>();
//    bool[,] visited = new bool[MapSettings.Width + 1, MapSettings.Height + 1];

//    for (int i = 0; i < MapSettings.Width; i++)
//        for (int j = 0; j < MapSettings.Height; j++)
//            visited[i, j] = false;

//    Queue<(Position, int)> bfsQueue = new Queue<(Position, int)>();
//    bfsQueue.Enqueue((new Position(entity.Position.X, entity.Position.Y), 0));
//    BFS(entities, visited, directions, radius, bfsQueue, entity);
//    return entities;
//}

//public void BFS(Entity source, Queue<(Position position, int depth)> queue, TraversalCell[,] vertices,
//    )

//// Implement algorithms via template method
//public void RetrieveClosestPlayer(Entity entity, int radius, out Queue<Direction> path, out Player player)
//{
//    List<Direction> directions = new List<Direction>() {Direction.West, Direction.North, Direction.East, Direction.South };
//    TraversalCell[,] vertices = new TraversalCell[2 * radius + 1, 2 * radius + 1];
//    Position position = entity.Position;

//    for (int i = 0; i < 2*radius + 1; i++)
//        for(int j = 0; j <  2*radius + 1; j++)
//            vertices[i,j] = new TraversalCell(new Position(position.X - (radius - i), position.Y - (radius - j)));

//    vertices[radius, radius].IsVisited = true;
//    Queue<(Position position , int depth)> bfsQueue = new Queue<(Position, int)>();
//    bfsQueue.Enqueue((position, 0));
//}




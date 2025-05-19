using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.HelperClasses;
using RPG_Game.Interfaces;
using RPG_Game.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Model;

public class Room : IGameState
{
    private RoomState RoomState;
    public int PlayerId { get; set; }
    public Room() { }
    public Room(RoomState roomState)
    {
        RoomState = roomState;
    }
    public Room(RoomState roomState, int playerID)
    {
        RoomState = roomState;
        PlayerId = playerID;
    }
    public bool AddObject(CellType cellType, Position position)
    {
        RoomState.Grid[position.X][position.Y].CellType |= cellType;
        return true;
    }
    public bool RemoveObject(CellType cellType, Position position)
    {
        if ((RoomState.Grid[position.X][position.Y].CellType & cellType) == 0)
            return false;

        if (RoomState.Grid[position.X][position.Y].Items?.Count == 0 || (cellType & CellType.Item) == 0)
            RoomState.Grid[position.X][position.Y].CellType &= ~cellType;

        return true;
    }
    public bool AddEntity(Entity entity, Position position)
    {
        if (!IsPosAvailable(position))
            return false;

        if (RoomState.Grid[position.X][position.Y].Entity != null)
            return false;

        AddObject(CellType.Enemy, position);
        entity.SetPosition(position);
        RoomState.Entities.Add(entity);
        RoomState.Grid[position.X][position.Y].Entity = entity;
        return true;
    }
    public void RemoveEntity(Entity entity)
    {
        Entity? removedEntity = RoomState.Grid[entity.Position.X][entity.Position.Y].Entity;
        if (removedEntity != null && removedEntity.Equals(entity))
        {
            RoomState.Grid[entity.Position.X][entity.Position.Y].Entity = null;
            RemoveObject(CellType.Enemy, entity.Position);
            RoomState.Entities.Remove(entity);
        }
    }
    public bool AddPlayer(Player player, Position position)
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
    public void RemovePlayer(Player player)
    {
        RoomState.ActivePlayers.TryGetValue(player.PlayerId, out Player? removedPlayer);
        if (removedPlayer != null /*&& removedPlayer.Equals(player)*/)
        {
            RoomState.Grid[player.Position.X][player.Position.Y].Entity = null;
            //RemoveObject(CellType.Player, player.Position);
            RoomState.Grid[player.Position.X][player.Position.Y].CellType &= ~CellType.Player;
            RoomState.ActivePlayers.Remove(player.PlayerId);
        }
    }
    public void AddItem(Item? item, Position position)
    {
        if (RoomState.Grid[position.X][position.Y].IsWall() || item == null)
            return;

        AddObject(CellType.Item, position);

        if (RoomState.Grid[position.X][position.Y].Items == null)
            RoomState.Grid[position.X][position.Y].Items = new List<Item>();

        RoomState.Grid[position.X][position.Y].Items!.Add(item);
    }
    public Item? RemoveItem(Position position, int index = 0)
    {
        if (RoomState.Grid[position.X][position.Y].Items == null 
            || RoomState.Grid[position.X][position.Y].Items?.Count == 0)
            return null;

        Item tempItem = RoomState.Grid[position.X][position.Y].Items!.ElementAt(index);
        RoomState.Grid[position.X][position.Y].Items!.RemoveAt(index);
        RemoveObject(CellType.Item, position);
        return tempItem;
    }
    public Item? RemoveItem(Position position, Item item)
    {
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
    public bool IsPosAvailable(Position position)
    {
        if (!IsInRange(position))
            return false;
        return RoomState.Grid[position.X][position.Y].IsWalkable();
    }
    public bool IsInRange(Position position)
    {
        if (position.X >= MapSettings.FrameSize
            && position.X < MapSettings.Width - MapSettings.FrameSize
            && position.Y >= MapSettings.FrameSize 
            && position.Y < MapSettings.Height - MapSettings.FrameSize)
            return true;
        return false;
    }
    public Entity? GetClosestEntity(Player source)
    {
        return RoomState.Entities
            .Concat(RoomState.ActivePlayers.Values
            .Where((player) => player.PlayerId != source.PlayerId))
            .MinBy((entity) => Math.Sqrt(Math.Pow(source.Position.X - entity.Position.X, 2) + Math.Pow(source.Position.Y - entity.Position.Y, 2)));
    }
    public void BFS(List<Entity> entities, bool[,] visited, List<(int x, int y)> directions,
        int maxDepth, Queue<(Position position, int depth)> bfsQueue, Entity? source)
    {
        while (bfsQueue.Count > 0)
        {
            (Position positon, int depth) = bfsQueue.Dequeue();
            if (depth > maxDepth)
                continue;

            Entity? entity = RoomState.Grid[positon.X][positon.Y].Entity;
            if (entity != null && !entity.Equals(source))
                entities.Add(entity);
            foreach (var direction in directions)
            {
                Position newPosition = new Position(positon.X + direction.x, positon.Y + direction.y);
                if (IsInRange(newPosition) && !RoomState.Grid[newPosition.X][newPosition.Y].IsWall() && !visited[newPosition.X, newPosition.Y])
                {
                    bfsQueue.Enqueue((newPosition, depth + 1));
                    visited[newPosition.X, newPosition.Y] = true;
                }
            }
        }
    }
    public List<Entity>? RetrieveEntitiesInRadius(Entity entity, int radius)
    {
        List<(int x, int y)> directions = new List<(int, int)> { (-1, 0), (1, 0), (0, 1), (0, -1) };
        List<Entity> entities = new List<Entity>();
        bool[,] visited = new bool[MapSettings.Width + 1, MapSettings.Height + 1];

        for (int i = 0; i < MapSettings.Width; i++)
            for (int j = 0; j < MapSettings.Height; j++)
                visited[i, j] = false;

        Queue<(Position, int)> bfsQueue = new Queue<(Position, int)>();
        bfsQueue.Enqueue((new Position(entity.Position.X, entity.Position.Y), 0));
        BFS(entities, visited, directions, radius, bfsQueue, entity);
        return entities;
    }
    // Note that no Copy was done, hence the references are dynamic
    // Introduce locking mechanism on printing
    public GameState GetGameState()
    {
        return new GameState(RoomState, PlayerId);
    }

    public RoomState GetRoomState() => RoomState;

    public Player GetPlayer()
    {
        return RoomState.ActivePlayers[PlayerId];
    }

    public List<Entity> GetVisibleEntities()
    {
        return RoomState.Entities.Concat(RoomState.ActivePlayers.Values
            .Where((player) => player.PlayerId != PlayerId))
            .ToList();
    }
    public List<Item>? GetItems(Position pos)
    {
        return RoomState.Grid[pos.X][pos.Y].Items;
    }
    public StringBuilder RenderMap()
    {
        return RoomState.RenderMap();
    }
}


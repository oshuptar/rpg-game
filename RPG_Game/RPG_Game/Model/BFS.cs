using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.Model.Entities;

namespace RPG_Game.Model;

// Default looks for all entities in the radius and reconstructs the path for the close one

public class TraversalCell
{
    public Position Coordinates { get; set; }
    public bool IsVisited { get; set; } = false;
    public Direction? PreviousStep { get; set; }
    public int? VisitedStep { get; set; }
    public TraversalCell(Position coordinates, Direction? previousStep = null)
    {
        Coordinates = coordinates;
        PreviousStep = previousStep;
    }
}

public class TemplateMethodBFS
{
    public static List<Direction> Directions { get; } = new() { Direction.West, Direction.North, Direction.East, Direction.South };
    public RoomState RoomState { get; }
    public TemplateMethodBFS(RoomState roomState)
    {
        RoomState = roomState;
    }

    public virtual bool AddEntity(Entity source, int depth,
        TraversalCell[,] traversalCells,
        Position absolutePosition,
        List<(Entity entity, int distance)> result)
    {
        try
        {
            RoomState.LockReadBlock(absolutePosition);
            Entity? entity = RoomState.Grid[absolutePosition.X][absolutePosition.Y].Entity;

            if (entity != null && !entity.Equals(source))
                result.Add((entity, depth));

            return false;
        }
        finally { RoomState.UnlockReadBlock(absolutePosition); }
    }

    public virtual void Initialize(
        Entity source,
        int radius,
        out Queue<(Position position, int depth)> bfsQueue,
        out TraversalCell[,] traversalCells)
    {

        traversalCells = new TraversalCell[2 * radius + 1, 2 * radius + 1];
        Position position = source.Position;

        for (int i = 0; i < 2 * radius + 1; i++)
            for (int j = 0; j < 2 * radius + 1; j++)
                traversalCells[i, j] = new TraversalCell(new Position(position.X - (radius - i), position.Y - (radius - j)));

        traversalCells[radius, radius].IsVisited = true;
        bfsQueue = new Queue<(Position, int)>();
        bfsQueue.Enqueue((new Position(radius, radius), 0));

    }

    public virtual bool IsWithinCircle(int radius, Position position)
    {
        if (Math.Abs(position.X - radius) + Math.Abs(position.Y - radius) <= radius)
            return true;
        return false;
    }

    public virtual void BFS(
        Entity source,
        Queue<(Position position, int depth)> bfsQueue,
        TraversalCell[,] traversalCells,
        int maxDepth,
        List<(Entity entity, int distance)> result)
    {
        while (bfsQueue.Count > 0)
        {
            (Position position, int depth) = bfsQueue.Dequeue();
            if (depth > maxDepth)
                continue;

            Position absolutePosition = traversalCells[position.X, position.Y].Coordinates;
            if (AddEntity(source, depth, traversalCells, absolutePosition, result))
                break;

            foreach (var direction in Directions)
            {
                (int x, int y) vector = DirectionTranslator.TranslateDirection(direction);
                Position newPosition = new Position(position.X + vector.x, position.Y + vector.y);

                if (IsWithinCircle(maxDepth, newPosition))
                {
                    absolutePosition = traversalCells[newPosition.X, newPosition.Y].Coordinates;
                    if (MapSettings.IsInRange(absolutePosition) && !RoomState.Grid[absolutePosition.X][absolutePosition.Y].IsWall() && !traversalCells[newPosition.X, newPosition.Y].IsVisited)
                    {
                        bfsQueue.Enqueue((newPosition, depth + 1));
                        traversalCells[newPosition.X, newPosition.Y].IsVisited = true;
                        traversalCells[newPosition.X, newPosition.Y].PreviousStep = direction;
                    }
                }
            }
        }
    }
    public virtual void Finalize(TraversalCell[,] traversalCells, Entity source, Entity target, int radius, List<Direction> path)
    {
        Position relativePosition = new Position(target.Position.X - source.Position.X + radius, target.Position.Y - source.Position.Y + radius);
        while (traversalCells[relativePosition.X, relativePosition.Y].PreviousStep != null)
        {
            Direction direction = traversalCells[relativePosition.X, relativePosition.Y].PreviousStep.Value;
            path.Add(direction);
            (int x, int y) vector = DirectionTranslator.TranslateDirection(direction);
            relativePosition.X = relativePosition.X - vector.x;
            relativePosition.Y = relativePosition.Y - vector.y;
        }
        path.Reverse();
    }

    public virtual void RunBFS(Entity source,
        int radius,
        out List<(Entity entity, int distance)> result,
        out List<Direction> path)
    {
        TraversalCell[,] traversalCells;
        Queue<(Position position, int depth)> bfsQueue;
        result = new List<(Entity entity, int distance)>();
        path = new List<Direction>();

        Initialize(source, radius, out bfsQueue, out traversalCells);
        BFS(source, bfsQueue, traversalCells, radius, result);

        Entity? target = result.Count != 0 ? result.FirstOrDefault().entity : null;

        if (target != null)
            Finalize(traversalCells, source, target, radius, path);
    }
}

public class BFSClosestEntity : TemplateMethodBFS
{
    public BFSClosestEntity(RoomState roomState) : base(roomState)
    {
    }

    public override bool AddEntity(Entity source, int depth, TraversalCell[,] traversalCells, Position absolutePosition, List<(Entity entity, int distance)> result)
    {
        try
        {
            RoomState.LockReadBlock(absolutePosition);

            Entity? entity = RoomState.Grid[absolutePosition.X][absolutePosition.Y].Entity;

            if (entity != null && !entity.Equals(source))
            {
                result.Add((entity, depth));
                return true;
            }
            return false;
        }
        finally { RoomState.UnlockReadBlock(absolutePosition); }
    }
}

public class BFSClosestPlayer : TemplateMethodBFS
{
    public BFSClosestPlayer(RoomState roomState) : base(roomState)
    {
    }

    public override bool AddEntity(Entity source, int depth, TraversalCell[,] traversalCells, Position absolutePosition, List<(Entity entity, int distance)> result)
    {
        try
        {
            RoomState.LockReadBlock(absolutePosition);

            Player? player = RoomState.Grid[absolutePosition.X][absolutePosition.Y].Entity as Player;
            if (player != null && !player.Equals(source))
            {
                result.Add((player, depth));
                return true;
            }
            return false;
        }
        finally { RoomState.UnlockReadBlock(absolutePosition); }
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Enums;
using RPG_Game.Model;
using RPG_Game.Model.Entities;
using static System.Collections.Specialized.BitVector32;

namespace RPG_Game.Controller.Server;

public interface IEnemyStrategy
{
    public void ExecuteStrategy(AuthorityGameState gameState, Entity entity);
}

public interface IAutonomousStrategy : IEnemyStrategy
{

}

public interface IReactiveStrategy : IEnemyStrategy
{
    public void SetEnemyState(IEnemyState enemyState);
    public void Move(AuthorityGameState gameState, Entity entity);
    public void SetPath(List<Direction> path);
    public void SetLockOnPlayer(Player player);
}

public class CalmEnemyStrategy : IAutonomousStrategy
{
    public virtual void ExecuteStrategy(AuthorityGameState gameState, Entity entity)
    {
        // does not move
    }
}

public abstract class ReactiveEnemyStrategy: IReactiveStrategy
{
    public object Lock = new object();

    public const int SeekRadius = 5;
    public List<Direction> Path { get; protected set; }
    public IEnemyState EnemyState { get; protected set; }
    public Player LockedPlayer { get; set; }
    public ReactiveEnemyStrategy()
    {
        EnemyState = new SeekState(this);
    }
    public virtual void ExecuteStrategy(AuthorityGameState gameState, Entity entity)
    {
        EnemyState.Execute(gameState, entity);
    }
    public virtual void SetEnemyState(IEnemyState enemyState)
    {
        EnemyState = enemyState;
    }
    public virtual void OnLockedPlayerMove(object sender, MoveEventArgs e)
    {
        lock (Lock)
        {
            Path.Add(e.Direction);
        }
    }
    public virtual void SetLockOnPlayer(Player player)
    {
        if(LockedPlayer != null) 
            LockedPlayer.EntityMoved -= OnLockedPlayerMove;

        LockedPlayer = player;
        player.EntityMoved += OnLockedPlayerMove;
    }
    public abstract void Move(AuthorityGameState authorityGameState, Entity entity);
    public virtual void SetPath(List<Direction> path) => Path = path;
}

public class AggressiveEnemyStrategy : ReactiveEnemyStrategy
{
    // the logic for movement; call DFS to check for cycles, if detected - simplify the stack, otherwise simply move
    public override void Move(AuthorityGameState authorityGameState, Entity entity)
    {
        Direction? direction = null;
        RemoveCycles(entity);
        lock (Lock)
        {
            if (Path.Count == 1)
                entity.Attack(LockedPlayer);
            
            direction = Path.First();
        }

        if(entity.Move(direction.Value, authorityGameState))
            Path.RemoveAt(0);
    }

    // DFS to check for cycles
    public void RemoveCycles(Entity source)
    {
        if (Path.Count == 1) return;

        List<Direction> path;
        int pathLength = default;
        lock (Lock)
        {
            path = new(Path);
            pathLength = Path.Count();
        }
            
        TraversalCell[,] traversalCells = new TraversalCell[2 * pathLength + 1, 2 * pathLength + 1];
        for (int i = 0; i < 2 * pathLength + 1; i++)
            for (int j = 0; j < 2 * pathLength + 1; j++)
                traversalCells[i, j] = new TraversalCell(new Position(source.Position.X - (SeekRadius - i), source.Position.Y - (SeekRadius - i)));

        traversalCells[pathLength, pathLength].IsVisited = true;
        traversalCells[pathLength, pathLength].VisitedStep = 0;

        Position absolutePosition = source.Position;
        Position relativePosition = new Position(pathLength, pathLength);
        int counter = 0;
        while (path.Count != 1)
        {
            Direction direction = path.First();
            path.RemoveAt(0);
            counter++;

            (int X, int Y)vector = DirectionTranslator.TranslateDirection(direction);
            relativePosition.X += vector.X;
            relativePosition.Y += vector.Y;
            if (traversalCells[relativePosition.X, relativePosition.Y].IsVisited)
            {
                int startingIndex = traversalCells[relativePosition.X, relativePosition.Y].VisitedStep.Value;
                lock (Lock)
                {
                    Path.RemoveRange(startingIndex, (counter - startingIndex));
                }
                break;
            }

            traversalCells[relativePosition.X, relativePosition.Y].IsVisited = true;
            traversalCells[relativePosition.X, relativePosition.Y].VisitedStep = counter;
        }
    }

    public AggressiveEnemyStrategy(): base()
    {

    }
}

public class SkittishEnemyStrategy : ReactiveEnemyStrategy
{
    // the logic for movement
    public override void Move(AuthorityGameState authorityGameState, Entity entity)
    {
        Position vector = new Position(entity.Position.X - LockedPlayer.Position.X, entity.Position.Y - LockedPlayer.Position.Y);
        // Implement via A*
        
    }

    public void CalculatePath(Player player, Entity entity)
    {
        Position vector = new Position(entity.Position.X - player.Position.X, entity.Position.Y - player.Position.Y);


    }

    public SkittishEnemyStrategy(): base()
    {

    }
}




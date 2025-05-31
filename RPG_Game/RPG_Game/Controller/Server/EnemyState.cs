using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Enums;
using RPG_Game.Model;
using RPG_Game.Model.Entities;

namespace RPG_Game.Controller.Server;

public interface IEnemyState
{
    public void Execute(AuthorityGameState gameState, Entity entity);
    public void SetEnemyState(IEnemyState enemyState);
}
public abstract class EnemyState : IEnemyState
{
    protected IReactiveStrategy EnemyStrategy { get; set; }
    public abstract void Execute(AuthorityGameState gameState, Entity entity);
    public abstract void SetEnemyState(IEnemyState enemyState);
    public EnemyState(IReactiveStrategy enemyStrategy)
    {
        EnemyStrategy = enemyStrategy;
    }
}

public class SeekState : EnemyState
{
    public override void Execute(AuthorityGameState gameState, Entity entity)
    {
        BFSClosestPlayer algorithm = new BFSClosestPlayer(gameState.GetRoomState());
        algorithm.RunBFS(entity, ReactiveEnemyStrategy.SeekRadius, out var result, out var path);
        if (result.Count() == 0) return;
        
        Player? player = result.FirstOrDefault().entity as Player;
        if (player == null) return;
        
        EnemyStrategy.SetPath(path);
        EnemyStrategy.SetLockOnPlayer(player);
        SetEnemyState(new MoveState(this.EnemyStrategy));
    }
    public override void SetEnemyState(IEnemyState enemyState)
    {
        EnemyStrategy.SetEnemyState(enemyState);
    }
    public SeekState(IReactiveStrategy enemyStrategy): base(enemyStrategy)
    { 
    }
}

public class MoveState : EnemyState
{
    private int StepCount { get; set; } = 0;
    public override void Execute(AuthorityGameState gameState, Entity entity)
    {
        if (StepCount >= 10)
        {
            SetEnemyState(new SeekState(this.EnemyStrategy));
            return;
        }// if more than 10 steps was made and unsuccessful set the state
        EnemyStrategy.Move(gameState, entity);
    }
    public override void SetEnemyState(IEnemyState enemyState)
    {
        // For example, if player was reached and no attack was performed we set the state to seek again
        // If someone attacks us in the process we change the LockedPlayer to a different one
        StepCount = 0;
        EnemyStrategy.SetEnemyState(enemyState);
    }
    public MoveState(IReactiveStrategy reactiveStrategy) : base(reactiveStrategy)
    {
    }
}

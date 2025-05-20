using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.View;

// Divide this interface into a few others
public interface IView
{
    public void SetController(IController controller);
    public void SetGameState(IGameState gameState);
    public void ReadInput(CancellationToken cancellationToken);
    public void SendCommand(IViewCommand command);
    public void HandleCommand();
}

public abstract class View : IView
{
    public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
    protected BlockingCollection<IViewCommand> Commands = new BlockingCollection<IViewCommand>(new ConcurrentQueue<IViewCommand>());
    protected IController Controller { get; set; }
    protected IGameState GameState { get; set; }
    public IInputHandler GameInputHandler { get; set; }
    public virtual void SetGameState(IGameState gameState)
    {
        GameState = gameState;
    }
    public virtual void SetController(IController controller) => Controller = controller;
    public abstract void ReadInput(CancellationToken cancellationToken);
    public virtual void SendCommand(IViewCommand command)
    {
        Commands.Add(command);
    }
    public virtual void HandleCommand()
    {
        while(!CancellationTokenSource.Token.IsCancellationRequested)
            Commands.Take().Execute();
    }
    public View(bool isServer)
    {
        GameInputHandler = new KeyboardHandler(isServer);
    }
}
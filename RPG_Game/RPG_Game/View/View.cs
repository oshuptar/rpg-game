using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Model;
using RPG_Game.UIHandlers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.View;

// Divide this interface into a few others
public abstract class View : IView
{
    public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();

    protected BlockingCollection<IViewCommand> Commands = new BlockingCollection<IViewCommand>(new ConcurrentQueue<IViewCommand>());
    //public static int CurrentFocus { get; protected set; }
    //public static FocusType FocusOn { get; protected set; }
    protected IController Controller { get; set; }
    protected IGameState GameState { get; set; }
    public IInputHandler GameInputHandler { get; set; }
    //public abstract void HideControls();
    //public abstract void DisplayRoutine();
    //public abstract void WelcomeRoutine();
    public virtual void SetGameState(IGameState gameState)
    {
        GameState = gameState;
    }
    public virtual void SetController(IController controller) => Controller = controller;
    //public abstract void SetInventoryFocus();
    //public abstract void SetHandsFocus();
    //public abstract void ResetFocusType();
    //public abstract void ShiftCurrentFocus(Direction direction);
    //public abstract void ClearLogMessage();
    public abstract void ReadInput(CancellationToken cancellationToken);
    //{
    //    while (!cancellationToken.IsCancellationRequested)
    //    {
    //        RequestType? requestType = GameInputHandler.TranslateRequest();
    //        if (requestType != null)
    //        {
    //            new Request(
    //                requestType.Value,
    //                Controller,
    //                GameState.PlayerId,
    //                CurrentFocus,
    //                FocusOn).SendRequest();
    //        }
    //    }
    //}
    public void SendCommand(IViewCommand command)
    {
        Commands.Add(command);
    }
    public void HandleCommand()
    {
        while(!CancellationTokenSource.Token.IsCancellationRequested)
            Commands.Take().Execute();
    }
    //public virtual void EndRoutine(bool flag)
    //{
    //    CancellationTokenSource.Cancel();
    //}
    public View(bool isServer)
    {
        //CurrentFocus = 0;
        //FocusOn = FocusType.Room;
        GameInputHandler = new KeyboardHandler(isServer);
    }
}

// implement one abstract view common for server and client, which would define the main logic of how view should work
// and then create 2 more abstract classes ServerView and ClientView to implement the rest
// The controller interface should only work with IView with them, nothing more general
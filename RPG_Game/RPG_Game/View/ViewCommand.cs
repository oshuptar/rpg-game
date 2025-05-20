using RPG_Game.Enums;
using RPG_Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.View;

// Do not forget to set the View
//public abstract class ViewCommand : IViewCommand
//{
//    public View View { get; protected set; }
//    public ViewCommand() { }
//    public ViewCommand(View view)
//    {
//        View = view;
//    }
//    public abstract void Execute();
//    public virtual void SetView(View view)
//    {
//        View = view;
//    }
//}

public abstract class ServerViewCommand : IServerViewCommand
{
    public ServerView ServerView { get; protected set; }
    public ServerViewCommand() { }
    public ServerViewCommand(ServerView serverView)
    {
        ServerView = serverView;
    }
    public abstract void Execute();
    public virtual void SetView(ServerView serverView)
    {
        ServerView = serverView;
    }
}

public abstract class ClientViewCommand : IClientViewCommand
{
    public ClientView ClientView { get; protected set; }
    public ClientViewCommand() { }
    public ClientViewCommand(ClientView clientView)
    {
        ClientView = clientView;
    }
    public abstract void Execute();

    public virtual void SetView(ClientView clientView)
    {
        ClientView = clientView;
    }
}

public class InventoryFocusCommand : ClientViewCommand
{
    public InventoryFocusCommand() : base() { }
    public override void Execute()
    {
        ClientView.SetInventoryFocus();
    }
}
public class HandsFocusCommand : ClientViewCommand
{
    public HandsFocusCommand() : base() { }
    public override void Execute()
    {
        ClientView.SetHandsFocus();
    }
}

public class RoomFocusCommand : ClientViewCommand
{
    public RoomFocusCommand() : base() { }
    public override void Execute()
    {
        ClientView.ResetFocusType();
    }
}
public class ShiftFocusCommand : ClientViewCommand
{
    public Direction Direction { get; }
    public ShiftFocusCommand(Direction direction) : base()
    {
        Direction = direction;
    }
    public override void Execute()
    {
        ClientView.ShiftCurrentFocus(Direction);
    }
}
//public class ClearLogCommand : ClientViewCommand
//{
//    public ClearLogCommand() : base() { }
//    public override void Execute()
//    {
//        ClientView.ClearLogMessage();
//    }
//}
public class HideControlsCommand : ClientViewCommand
{
    public HideControlsCommand() : base() { }
    public override void Execute()
    {
        ClientView.HideControls();
    }
} 

public class QuitCommand : ClientViewCommand
{
    public QuitCommand() : base()
    {
    }
    public override void Execute()
    {
        ClientView.EndRoutine(false);
    }
}
public class ResponseCommand : ClientViewCommand
{
    public IGameState GameState { get; }
    public ResponseCommand(IGameState gameState) : base()
    {
        GameState = gameState;
    }
    public override void Execute()
    {
        ClientView.SetGameState(GameState);
    }
}

public class ServerStopCommand : ServerViewCommand
{
    public ServerStopCommand() : base() { }
    public override void Execute()
    {
        //ServerView.EndRoutine(true);
    }
}

//public class LogCommand : ViewCommand
//{
//    public ILogMessage LogMessage { get; }
//    public LogCommand(ILogMessage logMessage) : base()
//    {
//        LogMessage = logMessage;
//    }
//    public override void Execute()
//    {
//        View.LogMessage(LogMessage);
//    }

//}
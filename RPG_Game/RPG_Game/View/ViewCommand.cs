using RPG_Game.Enums;
using RPG_Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.View;

public abstract class ViewCommand : IViewCommand
{
    public View View { get; protected set; }
    public ViewCommand() { }
    public ViewCommand(View view)
    {
        View = view;
    }
    public abstract void Execute();
    public virtual void SetView(View view)
    {
        View = view;
    }
}
public class InventoryFocusCommand : ViewCommand
{
    public InventoryFocusCommand() : base() { }
    public override void Execute()
    {
        View.SetInventoryFocus();
    }
}
public class HandsFocusCommand : ViewCommand
{
    public HandsFocusCommand() : base() { }
    public override void Execute()
    {
        View.SetHandsFocus();
    }
}

public class RoomFocusCommand : ViewCommand
{
    public RoomFocusCommand() : base() { }
    public override void Execute()
    {
        View.ResetFocusType();
    }
}
public class ShiftFocusCommand : ViewCommand
{
    public Direction Direction { get; }
    public ShiftFocusCommand(Direction direction) : base()
    {
        Direction = direction;
    }
    public override void Execute()
    {
        View.ShiftCurrentFocus(Direction);
    }
}
public class ClearLogCommand : ViewCommand
{
    public ClearLogCommand() : base() { }

    public override void Execute()
    {
        View.ClearLogMessage();
    }
}
public class HideControlsCommand : ViewCommand
{
    public HideControlsCommand() : base() { }
    public override void Execute()
    {
        View.HideControls();
    }
} 

public class QuitCommand : ViewCommand
{
    public bool isClient { get; }
    public QuitCommand(bool isClient) : base()
    {
        this.isClient = isClient;
    }
    public override void Execute()
    {
        if(isClient)
            View.EndRoutine(true);
        if (!isClient) 
            View.EndRoutine(false);
    }
}

public class ServerStopCommand : ViewCommand
{
    public ServerStopCommand() : base() { }
    public override void Execute()
    {
        View.EndRoutine(true);
    }
}

public class ResponseCommand : ViewCommand
{
    public IGameState GameState { get; }
    public ResponseCommand(IGameState gameState) : base() 
    {
        GameState = gameState;
    }
    public override void Execute()
    {
        View.SetGameState(GameState);
    }
}
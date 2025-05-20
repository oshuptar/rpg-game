using RPG_Game.Controller;
using RPG_Game.Enums;
using RPG_Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.View;

// the controller should only care how to pass request to the view
public interface IClientView : IView
{
    public void HideControls();
    public void ClearLogMessage();
    public void WelcomeRoutine();
    public void DisplayRoutine();
    public void EndRoutine(bool flag);
    public void SetInventoryFocus();
    public void SetHandsFocus();
    public void ResetFocusType();
    public void ShiftCurrentFocus(Direction direction);
}
public abstract class ClientView : View, IClientView
{
    public static int CurrentFocus { get; protected set; }
    public static FocusType FocusOn { get; protected set; }
    protected ClientView(bool flag) : base(false)
    {
        CurrentFocus = 0;
        FocusOn = FocusType.Room;
    }
    public override void SetGameState(IGameState gameState)
    {
        base.SetGameState(gameState);
        DisplayRoutine();
    }
    public override void ReadInput(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            RequestType? requestType = GameInputHandler.TranslateRequest();
            if (requestType != null)
            {
                new Request(
                    requestType.Value,
                    Controller,
                    GameState.PlayerId,
                    CurrentFocus,
                    FocusOn).SendRequest();
            }
        }
    }
    public virtual void EndRoutine(bool flag)
    {
        CancellationTokenSource.Cancel();
    }
    public abstract void HideControls();
    public abstract void DisplayRoutine();
    public abstract void WelcomeRoutine();
    public abstract void SetInventoryFocus();
    public abstract void SetHandsFocus();
    public abstract void ResetFocusType();
    public abstract void ShiftCurrentFocus(Direction direction);
    public abstract void ClearLogMessage();
}

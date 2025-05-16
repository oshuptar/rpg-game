using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.UIHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.View;

public abstract class View : IView
{
    protected IController Controller { get; set; }
    protected IGameState? GameState { get; set; }
    public IInputHandler GameInputHandler { get; set; } = new KeyboardHandler();
    public abstract void DisplayRoutine();
    public abstract void WelcomeRoutine();
    //public SendRequest(RequestType type)
    public View() { }
    public virtual void SetRoomState(IGameState gameState) => GameState = gameState;
    public virtual void SetController(IController controller) => Controller = controller;
}

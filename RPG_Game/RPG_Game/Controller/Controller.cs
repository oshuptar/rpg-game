using RPG_Game.UIHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public abstract class Controller : IController
{
    protected IView View { get; set; }
    protected IGameState GameState { get; set; }
    public Controller(IView view, IGameState gameState)
    {
        View = view;
        GameState = gameState;
    }
    public virtual void SetView(IView view)
    {
        View = view;
    }
    public virtual void SetGameState(IGameState gameState)
    {
        GameState = gameState;
    }

    public virtual void SetViewController()
    {
        View.SetController(this);
    }
}

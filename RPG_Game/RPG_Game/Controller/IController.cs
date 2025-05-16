using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.UIHandlers;
using RPG_Game.View;

namespace RPG_Game.Controller;

public interface IController
{
    public void SetView(IView view);
    public void SetGameState(IGameState gameState);
    public void SetViewController();
    public void ProcessRequest(IRequest request);
}

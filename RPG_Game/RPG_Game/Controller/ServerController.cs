using RPG_Game.UIHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ServerController : Controller
{
    private ServerHandlerChain ServerChain = ServerHandlerChain.GetInstance();
    public ServerController(IView view, IGameState gameState) : base(view, gameState)
    {
    }
    public virtual void HandleRequest(ActionRequest request)
    {
        ServerChain.HandleRequest(request);
    }
}

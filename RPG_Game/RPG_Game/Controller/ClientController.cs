using RPG_Game.UIHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ClientController : Controller
{
    public ClientController(IView view, IGameState gameState) : base(view, gameState)
    {
    }
}

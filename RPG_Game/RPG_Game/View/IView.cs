using RPG_Game.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Entities;

namespace RPG_Game.UIHandlers;

// Will define contract on UIHandlers
public interface IView
{
    public void WelcomeRoutine();
    public void DisplayRoutine();
    public void SetController(IController controller);
    public void SetRoomState(IGameState gameState);
}

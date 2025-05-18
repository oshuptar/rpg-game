using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Model;
using RPG_Game.UIHandlers;
using RPG_Game.View;

namespace RPG_Game.Controller;

public interface IController
{
    public void SetView(View.View view);
    public void SetGameState(Room gameState);
    public GameState GetGameState();
    public void SetViewController();
    public void HandleRequest(CancellationToken cancellationToken);
    public void SendRequest(IRequest request);
    public void HandleResponse(IResponse response);
}

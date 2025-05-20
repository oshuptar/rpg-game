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
    //public void SetView(View.View view);
    //public void SetGameState(Room gameState);
    //public GameState GetGameState();
    public void SetViewController();
    public void SendRequest(IRequest request);
    public void HandleRequest(CancellationToken cancellationToken);
    public void HandleResponse(IResponse response);
    //public void Run();
}

public interface IServerController : IController
{
    public void Listen(CancellationToken cancellationToken, int playerId);
    public void SetView(ServerView view);
    public void SetGameState(AuthorityGameState gameState);
    public void SendResponse(CancellationToken cancellationToken);
}

public interface IClientController : IController
{
    public void Listen(CancellationToken cancellationToken);
    public void SetView(ClientView view);
    public void SetGameState(GameState gameState);
}


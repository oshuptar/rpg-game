using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public abstract class ClientState : IClientState
{
    protected GameClient gameClient { get; set; }
    public ClientState(GameClient gameClient)
    {
        this.gameClient = gameClient;
    }
    public abstract void HostGame();
    public abstract void SetGameClientState();
}

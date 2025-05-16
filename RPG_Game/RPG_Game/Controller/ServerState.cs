using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public abstract class ServerState : IServerState
{
    protected GameServer gameServer { get; set; }
    public ServerState(GameServer gameServer)
    {
        this.gameServer = gameServer;
    }
    public abstract void HostGame();
    public abstract void SetGameServerState();
}

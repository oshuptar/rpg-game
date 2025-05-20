using RPG_Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ServerRun : ServerState
{
    public ServerRun(GameServer gameServer) : base(gameServer) { }
    public override void HostGame()
    {
        gameServer.ServerController.ServerRun();
        SetGameServerState();
    }
    public override void SetGameServerState()
    {
        gameServer.ServerState = new ServerEnd(gameServer);
    }
}

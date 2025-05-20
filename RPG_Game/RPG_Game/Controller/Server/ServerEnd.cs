using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ServerEnd : ServerState
{
    public ServerEnd(GameServer gameServer) : base(gameServer) { }
    public override void HostGame()
    {
        foreach(var client in gameServer.ServerController.ActiveClients.Values)
        {
            client.Close();
            client.Dispose();
        }
        gameServer.Server.Dispose();
        SetGameServerState();
    }
    public override void SetGameServerState()
    {
        Environment.Exit(0);
    }
}

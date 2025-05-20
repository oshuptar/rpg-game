using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ServerCrashed : ServerState
{
    public ServerCrashed(GameServer gameServer): base(gameServer) { }
    public override void HostGame()
    {
        gameServer.Server.Stop();
        Console.WriteLine("Server crashed: closing...");
        SetGameServerState();
    }
    public override void SetGameServerState()
    {
        gameServer.ServerState = new ServerEnd(gameServer);
    }
}

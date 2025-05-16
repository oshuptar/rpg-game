using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ServerStart : ServerState
{
    public ServerStart(GameServer gameServer) : base(gameServer)
    {
    }
    public override void HostGame()
    {
        var port = this.gameServer.PortNumber;
        var ipAddress = this.gameServer.IpAddress;

        gameServer.Server = new TcpListener(ipAddress, port);
        gameServer.Server.Start();
        this.SetGameServerState();
    }
    public override void SetGameServerState()
    {
        gameServer.ServerState = new ServerConfigureMap(gameServer);
    }
}

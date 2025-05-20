using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace RPG_Game.Controller;

public class ClientConnect : ClientState
{
    public ClientConnect(GameClient gameClient) : base(gameClient) { }
    public override void HostGame()
    {
        int portNumber = gameClient.PortNumber;
        IPAddress ipAddress = gameClient.IpAddress;

        gameClient.Client = new TcpClient(ipAddress.ToString(), portNumber);
        gameClient.ClientController.SetNetworkStream(gameClient.Client);
        SetGameClientState();
    }
    public override void SetGameClientState()
    {
        gameClient.ClientState = new ClientRun(gameClient);
    }
}
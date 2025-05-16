using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace RPG_Game.Controller;

public class ClientConnect : ClientState
{
    public ClientConnect(GameClient gameClient) : base(gameClient) { }
    public override void HostGame()
    {
        int portNumber = gameClient.PortNumber;
        IPAddress ipAddress = gameClient.IpAddress;

        Console.WriteLine("Connecting to server...");
        gameClient.Client = new TcpClient(ipAddress.ToString(), portNumber);
        Console.WriteLine("Connected to server!");

        //using NetworkStream stream = gameClient.Client.GetStream();

        //string message = "Hello";
        //byte[] data = Encoding.UTF8.GetBytes(message);
        //stream.Write(data, 0, data.Length);

        //Console.WriteLine("Message sent.");
    }

    public override void SetGameClientState()
    {
        throw new NotImplementedException();
    }
}

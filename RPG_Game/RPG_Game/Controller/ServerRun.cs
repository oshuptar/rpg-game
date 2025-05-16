using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ServerRun : ServerState
{
    static int playerId = 1;
    public ServerRun(GameServer gameServer) : base(gameServer) { }
    public override void HostGame()
    {
        // Accepts new connections
        // Runs Game Loop
        TcpListener server = gameServer.Server;

        while (true)
        {
            Console.WriteLine("Waiting for connection...");
            TcpClient client = server.AcceptTcpClient();

            Console.WriteLine($"Client {playerId} connected");

            gameServer.Clients.TryAdd(playerId, client);
            playerId++;
        }

        //Console.WriteLine("Waiting for connection...");
        //TcpClient client = server.AcceptTcpClient();
        //Console.WriteLine("Connected");

        //using NetworkStream stream = client.GetStream();
        //byte[] buffer = new byte[1024];
        //int bytesRead = stream.Read(buffer, 0, buffer.Length);
        //string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        //Console.WriteLine($"Received: {message}");
    }
    public override void SetGameServerState()
    {
        throw new NotImplementedException();
    }
}

using RPG_Game.Controller;
using System;
using System.Diagnostics;
using System.Net;

namespace RPG_Game;

internal class Program
{
    static async Task Main(string[] args)
    {
        IPAddress? ipAddress;
        if (args.Length == 2 && args[0] == "--server")
        {
            if (!int.TryParse(args[1], out int serverPort))
            {
                Console.WriteLine("Usage: --server <port>");
                return;
            }
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string sIpAddress = await client.GetStringAsync("https://api.ipify.org");
                    IPAddress.TryParse(sIpAddress, out ipAddress);
                    Console.WriteLine($"Server starting on public IP: {sIpAddress}:{serverPort}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to get public IP address: {ex.Message}\nTry Again Later!");
                }
            }
            //Process.Start(new ProcessStartInfo
            //{
            //    FileName = Environment.ProcessPath!,
            //    Arguments = $"--client 127.0.0.1 {serverPort}",
            //    UseShellExecute = true
            //});
            GameSession session = new GameSession();
            session.SetServer(new GameServer(IPAddress.Any, serverPort));
            return;
        }
        else if(args.Length == 3 && args[0] == "--client")
        {
            if (!IPAddress.TryParse(args[1], out ipAddress) || !int.TryParse(args[2], out int serverPort))
            {
                Console.WriteLine("Usage: --server <ipaddress> <port>");
                return;
            }
            Console.WriteLine($"Connecting to {ipAddress.ToString()}:{serverPort}");
            (new GameSession()).SetClient(new GameClient(ipAddress, serverPort));
        }

        bool isServer = false;
        GameSession gameSession = new GameSession();

        Console.WriteLine("Welcome to the RPG Game!\nStart as (S)erver or (C)lient?");
        string? input = Console.ReadLine();
        input = input?.ToUpper();


        if (input != null && input.Equals("S"))
            isServer = true;
        else if (input != null && !input.Equals("C"))
        {
            Console.WriteLine("Invalid input!");
            return;
        }

        Console.WriteLine("Enter the port number:");
        input = Console.ReadLine();

        int.TryParse(input, out var port);
        if (port <= 0 || port >= 65535)
        {
            Console.WriteLine("Invalid port number!");
            return;
        }

        if (isServer)
        {
            ipAddress = IPAddress.Any;
            string processPath = Environment.ProcessPath!;
            Process.Start(new ProcessStartInfo
            {
                FileName = processPath,
                Arguments = $"--server {port}",
                UseShellExecute = true,
                CreateNoWindow = false
            });

            gameSession.SetClient(new GameClient(IPAddress.Loopback, port));
        }
        else
        {
            Console.WriteLine("Enter IPv4 address:");
            input = Console.ReadLine();
            IPAddress.TryParse(input, out ipAddress);

            if (ipAddress == null)
            {
                Console.WriteLine("Invalid IP address!");
                return;
            }

            Console.WriteLine($"Connecting to {ipAddress.ToString()}:{port}");

            gameSession.SetClient(new GameClient(ipAddress, port));
        }
    }
}


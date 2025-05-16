using RPG_Game.Controller;
using System;
using System.Net;

namespace RPG_Game;

internal class Program
{
    static async Task Main(string[] args)
    {
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

        IPAddress? ipAddress;
        if (isServer)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string sIpAddress = await client.GetStringAsync("https://api.ipify.org");
                    IPAddress.TryParse(sIpAddress, out ipAddress);
                    Console.WriteLine($"Server starting on public IP: {sIpAddress}:{port}");

                    ipAddress = IPAddress.Any;
                    gameSession.SetServer(new GameServer(ipAddress, port));
                    gameSession.SetClient(new GameClient(IPAddress.Loopback, port));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to get public IP address: {ex.Message}\nTry Again Later!");
                }
            }
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

            // TODO: Comment
            gameSession.SetClient(new GameClient(IPAddress.Loopback, port));
        }
    }
}


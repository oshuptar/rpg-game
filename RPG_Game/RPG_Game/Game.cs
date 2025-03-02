using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public class Game
{
    private Room _room = new Room();

    public Game()
    {

    }

    public void StartGame()
    {

        Player player = new Player();

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key; // Retrieves the key

                switch (key)
                {
                    case ConsoleKey.W:
                        player.Move(Direction.Up, this._room);
                        break;
                    case ConsoleKey.S:
                        player.Move(Direction.Down, this._room);
                        break;
                    case ConsoleKey.A:
                        player.Move(Direction.Left, this._room);
                        break;
                    case ConsoleKey.D:
                        player.Move(Direction.Right, this._room);
                        break;
                }
            }

        }
    }
}


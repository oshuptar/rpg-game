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

    public void StartGame()
    {

        Player player = new Player();

        Console.WriteLine("Moves in four directions are controlled by `W`, `S`, `A`, `D`");
        Console.WriteLine("To Start the Game press any key");
        Console.WriteLine("Use arrows in order to change the desired item to pickup");

        while (true) // How to make a smooth output?
        {
            if (Console.KeyAvailable)
            {
 
                var key = Console.ReadKey(true).Key; // Retrieves the key

                //Improve displaying of current chosen object
                switch (key)
                {
                    case ConsoleKey.W:
                        player.Move(Direction.Up, this._room);
                        ObjectDisplayer.ResetFocus();
                        break;
                    case ConsoleKey.S:
                        player.Move(Direction.Down, this._room);
                        ObjectDisplayer.ResetFocus();
                        break;
                    case ConsoleKey.A:
                        player.Move(Direction.Left, this._room);
                        ObjectDisplayer.ResetFocus();
                        break;
                    case ConsoleKey.D:
                        player.Move(Direction.Right, this._room);
                        ObjectDisplayer.ResetFocus();
                        break;
                    case ConsoleKey.E:
                        //To do
                        // Items can be choosen by index
                        IItem? item = _room.RemoveItem(player.Position, ObjectDisplayer.CurrentFocus);
                        item?.PickUp(player);
                        ObjectDisplayer.ResetFocus();
                        break;
                    case ConsoleKey.RightArrow:
                        ObjectDisplayer.IncreaseCurrentFocus(_room, player.Position);
                        break;
                    case ConsoleKey.LeftArrow:
                        ObjectDisplayer.DecreaseCurrentFocus(_room, player.Position);
                        break;
                }
                Thread.Sleep(1); //Ensures smoothness

                Console.Clear(); // Clear the screen before printing the new grid
                //Console.SetCursorPosition(0, 0);
                ObjectDisplayer.PrintGrid(_room); // Print the grid
                ObjectDisplayer.DisplayTileItems(_room, player.Position);
                ObjectDisplayer.DisplayCurrent(_room, player.Position);
                
            }

        }
    }
}


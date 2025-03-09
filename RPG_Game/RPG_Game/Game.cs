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
        Console.WriteLine("To drop an item press 'G'");
        Console.WriteLine("Use arrows to navigate through inventory, map items or eqquiped items");
        Console.WriteLine("To enter the inventory scope - press 'I', then use arrows to change the object");
        Console.WriteLine("To enter hands scope - press 'H', then use arrows to change the object");
        Console.WriteLine("To leave hands or inventory scope - press 'Escape'");
        Console.WriteLine("To Start the Game press any key");
        Console.WriteLine("Have fun!");


        while (true) // How to make a smooth output?
        {
            if (Console.KeyAvailable)
            {
                  // aDD rOUTINES.cS do a routine for moving
                var key = Console.ReadKey(true).Key; // Retrieves the key

                //Improve displaying of current chosen object
                switch (key)
                {
                    case ConsoleKey.W:
                        player.Move(Direction.Up, this._room);
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.S:
                        player.Move(Direction.Down, this._room);
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.A:
                        player.Move(Direction.Left, this._room);
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.D:
                        player.Move(Direction.Right, this._room);
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.E:
                        IItem? item = _room.RemoveItem(player.Position, ObjectDisplayer.CurrentFocus);
                        item?.PickUp(player);
                        ObjectDisplayer.ResetFocusIndex();
                        break;

                    //Objects can be dropped from inventory and from hands
                    case ConsoleKey.G when (ObjectDisplayer.FocusOn == FocusType.Inventory || ObjectDisplayer.FocusOn == FocusType.Hands):
                        IItem? temp = player.Drop(this._room, ObjectDisplayer.CurrentFocus);
                        //_room.AddItem(temp, player.Position);
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.I:
                        // to Enter inventory; changes the behaviour of arrows
                        ObjectDisplayer.SetInventoryFocus();
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.H:
                        ObjectDisplayer.SetHandsFocus();
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.Escape:
                        ObjectDisplayer.ResetFocusType();
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.RightArrow:
                        ObjectDisplayer.ShiftCurrentFocus(_room, player, Direction.Right);
                        break;
                    case ConsoleKey.LeftArrow:
                        ObjectDisplayer.ShiftCurrentFocus(_room, player, Direction.Left);
                        break;
                }
                Thread.Sleep(1); //Ensures smoothness

                Console.Clear(); // Clear the screen before printing the new grid
                //Console.SetCursorPosition(0, 0);
                ObjectDisplayer.DisplayRoutine(_room, player);
                
            }

        }
    }
}


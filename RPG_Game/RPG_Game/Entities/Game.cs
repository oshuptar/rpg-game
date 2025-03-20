using RPG_Game.Enums;
using RPG_Game.HelperClasses;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entiities;

public class Game
{
    private Room _room = new Room();

    public void StartGame()
    {
        Player player = new Player();
        ObjectDisplayer.DisplayControls();
        MapBuilder map = new MapBuilder();

        map.CreateEmptyDungeon();
        map.FillDungeon();
        map.AddCentralRoom();
        map.AddChambers();
        map.AddPaths();

        _room = map.GetResult();
        Console.WriteLine(" - To Start the Game press any key");
        Console.WriteLine("Have fun!");

        IItem? item;

        int i = 0;
        while (true)
        {
            if (Console.KeyAvailable)
            {
                // Add routines for every key
                var key = Console.ReadKey(true).Key; // Retrieves the key

                if (i == 0)
                    Console.Clear();

                switch (key)
                {
                    case ConsoleKey.W:
                        player.Move(Direction.Up, _room);
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.S:
                        player.Move(Direction.Down, _room);
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.A:
                        player.Move(Direction.Left, _room);
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.D:
                        player.Move(Direction.Right, _room);
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.E:
                        item = _room.RemoveItem(player.Position, ObjectDisplayer.CurrentFocus);
                        player.PickUp(item);
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.G when ObjectDisplayer.FocusOn == FocusType.Inventory: //Objects can be dropped from inventory and from hands
                        item = player.Drop(_room, ObjectDisplayer.CurrentFocus, true);
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.G when ObjectDisplayer.FocusOn == FocusType.Hands:
                        item = player.Drop(_room, ObjectDisplayer.CurrentFocus, false);
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.I: // to Enter inventory; changes the behaviour of arrows
                        ObjectDisplayer.SetInventoryFocus();
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.H:
                        ObjectDisplayer.SetHandsFocus();
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.Q when ObjectDisplayer.FocusOn == FocusType.Inventory:

                        item = player.Retrieve(ObjectDisplayer.CurrentFocus, true);
                        if(player.Equip(item)) 
                            item = player.Remove(ObjectDisplayer.CurrentFocus, true);
                        ObjectDisplayer.ResetFocusIndex();
                        break;

                    case ConsoleKey.Q when ObjectDisplayer.FocusOn == FocusType.Room:
                        item = _room.RemoveItem(player.Position, ObjectDisplayer.CurrentFocus);
                        if (!player.Equip(item, false))
                            _room.AddItem(item, player.Position);
                        ObjectDisplayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.Q when ObjectDisplayer.FocusOn == FocusType.Hands:
                        player.UnEquip(ObjectDisplayer.CurrentFocus);
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
                    case ConsoleKey.K:
                        ObjectDisplayer.ChangeControlsVisibility();
                        break;
                }
                //Thread.Sleep(1); //Ensures smoothness
                ObjectDisplayer.DisplayRoutine(_room, player);

                i++;
            }

        }
    }
}


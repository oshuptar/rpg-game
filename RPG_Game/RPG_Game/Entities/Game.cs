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
        Console.SetWindowSize(Console.LargestWindowWidth - 50, Console.LargestWindowHeight - 10);
        Console.WindowLeft = 0;
        Console.WindowTop = 0;

        Player player = new Player();
        ObjectDisplayer displayer = ObjectDisplayer.GetInstance();
        MapBuilder map = new MapBuilder();

        map.CreateEmptyDungeon();
        map.FillDungeon();
        map.AddCentralRoom();
        //map.AddChambers();
        map.AddPaths(); // Paths do not make any sense w/o chambers and central room
        map.PlaceItems();
        map.PlaceDecoratedWeapons();
        map.PlaceDecoratedItems();
        map.PlaceDecoratedItems();
        map.SpawnPlayer(player);
        _room = map.GetResult();

        displayer.WelcomeRoutine();

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
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.S:
                        player.Move(Direction.Down, _room);
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.A:
                        player.Move(Direction.Left, _room);
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.D:
                        player.Move(Direction.Right, _room);
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.E:
                        item = _room.RemoveItem(player.Position, ObjectDisplayer.CurrentFocus);
                        player.PickUp(item);
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.G when ObjectDisplayer.FocusOn == FocusType.Inventory: //Objects can be dropped from inventory and from hands
                        item = player.Drop(_room, ObjectDisplayer.CurrentFocus, true);
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.G when ObjectDisplayer.FocusOn == FocusType.Hands:
                        item = player.Drop(_room, ObjectDisplayer.CurrentFocus, false);
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.I: // to Enter inventory; changes the behaviour of arrows
                        displayer.SetInventoryFocus();
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.H:
                        displayer.SetHandsFocus();
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.Q when ObjectDisplayer.FocusOn == FocusType.Inventory:

                        item = player.Retrieve(ObjectDisplayer.CurrentFocus, true);
                        if(player.Equip(item)) 
                            item = player.Remove(ObjectDisplayer.CurrentFocus, true);
                        displayer.ResetFocusIndex();
                        break;

                    case ConsoleKey.Q when ObjectDisplayer.FocusOn == FocusType.Room:
                        item = _room.RemoveItem(player.Position, ObjectDisplayer.CurrentFocus);
                        if (!player.Equip(item, false))
                            _room.AddItem(item, player.Position);
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.Q when ObjectDisplayer.FocusOn == FocusType.Hands:
                        player.UnEquip(ObjectDisplayer.CurrentFocus);
                        break;
                    case ConsoleKey.Escape:
                        displayer.ResetFocusType();
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.RightArrow:
                        displayer.ShiftCurrentFocus(_room, player, Direction.Right);
                        break;
                    case ConsoleKey.LeftArrow:
                        displayer.ShiftCurrentFocus(_room, player, Direction.Left);
                        break;
                    case ConsoleKey.K:
                        displayer.ChangeControlsVisibility();
                        break;
                }
                displayer.DisplayRoutine(_room, player);
                i++;
            }

        }
    }
}


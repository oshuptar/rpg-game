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

    private void SetMapConfiguration(MapConfigurator mapConfigurator)
    {
        _room = mapConfigurator.GetResult();
        ObjectRenderer.GetInstance().SetMapInstructionConfigurator(mapConfigurator.GetInstructionConfiguration());
        ObjectDisplayer.GetInstance().SetRoom(_room);
    }

    public void StartGame()
    {
        Console.SetWindowSize(Console.LargestWindowWidth - 50, Console.LargestWindowHeight - 10);
        Console.WindowLeft = 0;
        Console.WindowTop = 0;

        Player player = new Player();
        ObjectDisplayer displayer = ObjectDisplayer.GetInstance();
        MapConfigurator map = new MapConfigurator();

        // This will kind of allow to change the map configuration during runtime
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

        SetMapConfiguration(map);

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
                        item = _room.RemoveItem(player.Position, displayer.CurrentFocus);
                        player.PickUp(item);
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.G when displayer.FocusOn == FocusType.Inventory: //Objects can be dropped from inventory and from hands
                        item = player.Drop(_room, displayer.CurrentFocus, true);
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.G when displayer.FocusOn == FocusType.Hands:
                        item = player.Drop(_room, displayer.CurrentFocus, false);
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
                    case ConsoleKey.Q when displayer.FocusOn == FocusType.Inventory:

                        item = player.Retrieve(displayer.CurrentFocus, true);
                        if(player.Equip(item)) 
                            item = player.Remove(displayer.CurrentFocus, true);
                        displayer.ResetFocusIndex();
                        break;

                    case ConsoleKey.Q when displayer.FocusOn == FocusType.Room:
                        item = _room.RemoveItem(player.Position, displayer.CurrentFocus);
                        if (!player.Equip(item, false))
                            _room.AddItem(item, player.Position);
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.Q when displayer.FocusOn == FocusType.Hands:
                        player.UnEquip(displayer.CurrentFocus);
                        break;
                    case ConsoleKey.Escape:
                        displayer.ResetFocusType();
                        displayer.ResetFocusIndex();
                        break;
                    case ConsoleKey.RightArrow:
                        displayer.ShiftCurrentFocus(player, Direction.Right);
                        break;
                    case ConsoleKey.LeftArrow:
                        displayer.ShiftCurrentFocus(player, Direction.Left);
                        break;
                    case ConsoleKey.K:
                        displayer.ChangeControlsVisibility();
                        break;
                }
                displayer.DisplayRoutine(player);
                i++;
            }

        }
    }
}


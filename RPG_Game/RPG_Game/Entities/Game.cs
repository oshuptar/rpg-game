using RPG_Game.Enums;
using RPG_Game.HelperClasses;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.UIHandlers;
using RPG_Game.Controller;

namespace RPG_Game.Entiities;

public class Game
{
    private Room _room = new Room();

    private void SetMapConfiguration(MapConfigurator mapConfigurator)
    {
        _room = mapConfigurator.GetResult();
        ObjectRenderer.GetInstance().SetMapInstructionConfigurator(mapConfigurator.GetInstructionConfiguration());
        ConsoleObjectDisplayer.GetInstance().SetRoom(_room);
    }

    public Game() { }

    public void StartGame()
    {
        Console.SetWindowSize(Console.LargestWindowWidth - 50, Console.LargestWindowHeight - 10);
        Console.WindowLeft = 0;
        Console.WindowTop = 0;

        Player player = new Player();
        ConsoleObjectDisplayer displayer = ConsoleObjectDisplayer.GetInstance();
        MapConfigurator map = new MapConfigurator();

        // This will allow to change the map configuration during runtime
        map.CreateEmptyDungeon();
        map.FillDungeon();
        map.AddCentralRoom();
        map.AddChambers();
        map.AddPaths();
        map.PlaceItems();
        map.PlaceDecoratedWeapons();
        map.PlaceDecoratedItems();
        map.PlaceDecoratedItems();
        map.SpawnPlayer(player);
        map.PlaceEnemies();
        map.PlacePotions();
        SetMapConfiguration(map);
        displayer.WelcomeRoutine();

        KeyboardTranslator keyboardManager = new KeyboardTranslator();

        int i = 0;
        while (true)
        {
            RequestType? requestType = keyboardManager.TranslateRequest();
            if (requestType != null)
            {
                if (i == 0)
                    Console.Clear();

                keyboardManager.DispatchRequest(new ActionRequest(new Context(player, _room), (RequestType)requestType));
                displayer.DisplayRoutine(player);
                i++;
            }

        }
    }
}


// This is just to showcase that we can change map configuration during runtime
//if (i == 10)
//{
//    MapConfigurator map2 = new MapConfigurator();
//    map2.CreateEmptyDungeon();
//    map2.FillDungeon();
//    map2.AddCentralRoom();
//    map2.AddPaths(); // Paths do not make any sense w/o chambers and central room
//    map2.PlaceItems();
//    map2.PlaceDecoratedWeapons();
//    map2.SpawnPlayer(player);
//    this.SetMapConfiguration(map2);
//}

//if (Console.KeyAvailable)
//            {
//                // Add routines for every key
//                var key = Console.ReadKey(true).Key; // Retrieves the key

//                if (i == 0)
//                    Console.Clear();

//                switch (key)
//                {
//                    case ConsoleKey.W:
//                        player.Move(Direction.North, _room);
//                        displayer.ResetFocusIndex();
//                        break;
//                    case ConsoleKey.S:
//                        player.Move(Direction.South, _room);
//                        displayer.ResetFocusIndex();
//                        break;
//                    case ConsoleKey.A:
//                        player.Move(Direction.West, _room);
//                        displayer.ResetFocusIndex();
//                        break;
//                    case ConsoleKey.D:
//                        player.Move(Direction.East, _room);
//                        displayer.ResetFocusIndex();
//                        break;
//                    case ConsoleKey.E:
//                        item = _room.RemoveItem(player.Position, displayer.CurrentFocus);
//                        player.PickUp(item);
//                        displayer.ResetFocusIndex();
//                        break;
//                    case ConsoleKey.G when displayer.FocusOn == FocusType.Inventory: //Objects can be dropped from inventory and from hands
//                        item = player.Drop(_room, displayer.CurrentFocus, true);
//                        displayer.ResetFocusIndex();
//                        break;
//                    case ConsoleKey.G when displayer.FocusOn == FocusType.Hands:
//                        item = player.Drop(_room, displayer.CurrentFocus, false);
//                        displayer.ResetFocusIndex();
//                        break;
//                    case ConsoleKey.I: // to Enter inventory; changes the behaviour of arrows
//                        displayer.SetInventoryFocus();
//                        displayer.ResetFocusIndex();
//                        break;
//                    case ConsoleKey.H:
//                        displayer.SetHandsFocus();
//                        displayer.ResetFocusIndex();
//                        break;
//                    case ConsoleKey.Q when displayer.FocusOn == FocusType.Inventory:

//                        item = player.Retrieve(displayer.CurrentFocus, true);
//                        if(player.Equip(item)) 
//                            item = player.Remove(displayer.CurrentFocus, true);
//                        displayer.ResetFocusIndex();
//                        break;

//                    case ConsoleKey.Q when displayer.FocusOn == FocusType.Room:
//                        item = _room.RemoveItem(player.Position, displayer.CurrentFocus);
//                        if (!player.Equip(item, false))
//                            _room.AddItem(item, player.Position);
//                        displayer.ResetFocusIndex();
//                        break;
//                    case ConsoleKey.Q when displayer.FocusOn == FocusType.Hands:
//                        player.UnEquip(displayer.CurrentFocus);
//                        displayer.ResetFocusIndex();
//                        break;
//                    case ConsoleKey.Escape:
//                        displayer.ResetFocusType();
//                        displayer.ResetFocusIndex();
//                        break;
//                    case ConsoleKey.RightArrow:
//                        displayer.ShiftCurrentFocus(player, Direction.East);
//                        break;
//                    case ConsoleKey.LeftArrow:
//                        displayer.ShiftCurrentFocus(player, Direction.West);
//                        break;
//                    case ConsoleKey.K:
//                        displayer.ChangeControlsVisibility();
//                        break;
//                    default:
//                        break;
//                }
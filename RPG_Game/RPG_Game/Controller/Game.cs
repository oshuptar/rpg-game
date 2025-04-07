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
using RPG_Game.Entiities;

namespace RPG_Game.Controller;

public class Game
{
    private Room _room = new Room();
    private Player _player = new Player();
    private MapConfigurator _map = new MapConfigurator();
    private IInputHandler _inputHandler;
    private GamePhase _gamePhase;
    //private UIHandler _uiHandler;

    public Game()
    {
        ConfigureMap();
        ConfigureSettings();
    }
    private void SetMapConfiguration(MapConfigurator mapConfigurator)
    {
        _room = mapConfigurator.GetResult();
        ObjectRenderer.GetInstance().SetMapInstructionConfigurator(mapConfigurator.GetInstructionConfiguration());
        ConsoleObjectDisplayer.GetInstance().SetRoom(_room);
        // View must access the model state
    }

    public void ConfigureMap()
    {
        _map.CreateEmptyDungeon();
        _map.FillDungeon();
        _map.AddCentralRoom();
        _map.AddChambers();
        _map.AddPaths();
        _map.PlaceItems();
        _map.PlaceDecoratedWeapons();
        _map.PlaceDecoratedItems();
        _map.PlaceDecoratedItems();
        _map.SpawnPlayer(_player);
        _map.PlaceEnemies();
        _map.PlacePotions();
        SetMapConfiguration(_map);
    }

    public void ConfigureSettings()
    {
        _inputHandler = new KeyboardTranslator();
    }

    public void StartGame()
    {
        ConsoleObjectDisplayer displayer = ConsoleObjectDisplayer.GetInstance();
        displayer.WelcomeRoutine();
        _gamePhase = GamePhase.Welcome;
        while (true)
        {
            RequestType? requestType = _inputHandler.TranslateRequest();

            if (requestType == null)
                continue;

            if (_gamePhase == GamePhase.Welcome)
            {
                _gamePhase = GamePhase.Playing;
                Console.Clear();
            }

            _inputHandler.DispatchRequest(new ActionRequest(new Context(_player, _room), (RequestType)requestType));
            displayer.DisplayRoutine(_player);
        }
    }
}

public enum GamePhase
{
    Welcome,
    Playing
}


// This will allow to change the map configuration during runtime
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


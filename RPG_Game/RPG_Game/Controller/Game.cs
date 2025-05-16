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
using System.Runtime.CompilerServices;
using RPG_Game.Entities;
using RPG_Game.LogMessages;

namespace RPG_Game.Controller;

public enum GamePhase
{
    Welcome,
    Playing
}

// Per-session Per-Player class
// Most likely room later on has to contian a list of players

// Game must be per-user class. Encpsulate the MODEL and map configration to another class
public class Game
{
    private Room _room = new Room();
    private Player _player = new Player();
    private MapConfigurator _map = new MapConfigurator();
    private IInputHandler _inputHandler;
    private GamePhase _gamePhase;
    public AttackType AttackType { get; set; } = AttackType.NormalAttack;
    public AttackStrategy AttackStrategy { get; set; } = new NormalAttackStrategy();
    //private UIHandler _uiHandler;
    //List<Player> Players
    public Game()
    {
        _player.OwnDeath += PlayerDeathHandler;
        ConfigureMap();
        ConfigureSettings();
    }
    private void SetMapConfiguration(MapConfigurator mapConfigurator)
    {
        _room = mapConfigurator.GetResult();
        ObjectRenderer.GetInstance().SetMapInstructionConfigurator(mapConfigurator.GetInstructionConfiguration());
        ClientConsoleView.GetInstance().SetGameState(this);

        foreach (var enemy in _room.GetRoomState().Enemies)
            enemy.OwnDeath += EnemyDeathHandler;
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
        _inputHandler = new KeyboardHandler();
    }

    public void StartGame()
    {
        ClientConsoleView view = ClientConsoleView.GetInstance();
        view.WelcomeRoutine();
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
            _inputHandler.DispatchRequest(new ActionRequest(new Context(this), (RequestType)requestType));
            view.DisplayRoutine();
        }
    }

    public void SetAttackMode(AttackType attackType, AttackStrategy attackStrategy)
    {
        AttackType = attackType;
        AttackStrategy = attackStrategy;
    }

    public Room GetRoom() => _room;
    public Player GetPlayer() => _player;

    public void PlayerDeathHandler(object sender, EventArgs e)
    {
        ClientConsoleView.GetInstance().LogMessage(new OnPlayerDeathMessage((Player)sender));
        Thread.Sleep(3000);
        _inputHandler.DispatchRequest(new ActionRequest(new Context(this), RequestType.Quit));
    }

    public void EnemyDeathHandler(object sender, EventArgs e)
    {
        _room.RemoveEntity((IEnemy)sender);
        ClientConsoleView.GetInstance().LogMessage(new OnEnemyDeathMessage((IEnemy)sender));
    }
}

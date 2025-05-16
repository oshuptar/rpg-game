using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.LogMessages;
using RPG_Game.UIHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class GameClient
{
    public int PlayerId { get; set; }
    public IPAddress IpAddress { get; set; }
    public int PortNumber { get; set; }
    public TcpClient Client { get; set; }

    private IClientState _clientState;
    public IClientState ClientState
    {
        get => _clientState;
        set
        {
            _clientState = value;
            _clientState.HostGame();
        }
    }
    public ClientController ClientController { get; set; }

    public GameClient(IPAddress ipAddress, int portNumber)
    {
        IpAddress = ipAddress;
        PortNumber = portNumber;
        ClientController = new ClientController(ClientConsoleView.GetInstance(), new GameStateViewProxy());
        ClientController.SetViewController();
        ClientState = new ClientConnect(this);
    }

    private Player _player = new Player();
    public AttackType AttackType { get; set; } = AttackType.NormalAttack;
    public AttackStrategy AttackStrategy { get; set; } = new NormalAttackStrategy();
    //
    //private IView GameView;
    //private IInputHandler GameInputHandler;
    //public void ConfigureSettings()
    //{
    //    GameInputHandler = new KeyboardTranslator();
    //}

    public void PlayerDeathHandler(object sender, EventArgs e)
    {
        ClientConsoleView.GetInstance().LogMessage(new OnPlayerDeathMessage((Player)sender));
        Thread.Sleep(3000);
        //GameInputHandler.DispatchRequest(new ActionRequest(new Context(this), RequestType.Quit));
    }
    public void EnemyDeathHandler(object sender, EventArgs e)
    {
        //Room.RemoveEntity((IEnemy)sender);
        ClientConsoleView.GetInstance().LogMessage(new OnEnemyDeathMessage((IEnemy)sender));
    }
}

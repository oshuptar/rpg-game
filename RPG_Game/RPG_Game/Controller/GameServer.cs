using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.HelperClasses;
using RPG_Game.UIHandlers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RPG_Game.View;
using RPG_Game.Model;

namespace RPG_Game.Controller;
//TcpClient.GetStream() always returns the same stream instance tied to the connection. You don't create a new stream every time.
public class GameServer
{
    public IPAddress IpAddress { get; set; }
    public int PortNumber { get;set; }

    private IServerState _serverState;
    public IServerState ServerState
    {
        get => _serverState;
        set
        {
            _serverState = value;
            _serverState.HostGame();
        }
    }
    public ServerController ServerController { get; set; }
    public TcpListener Server { get; set; }
    public MapConfigurator Map { get; set; }
    public GameServer(IPAddress ipAddress, int portNumber) 
    { 
        IpAddress = ipAddress;
        PortNumber = portNumber;
        Map = new MapConfigurator();
        ServerController = new ServerController(new ServerConsoleView(), new Room());
        ServerController.SetViewController();

        ServerState = new ServerStart(this);
    }

    //private void SetMapConfiguration(MapConfigurator mapConfigurator)
    //{
    //    RoomState = mapConfigurator.GetResult();
    //    ObjectRenderer.GetInstance().SetMapInstructionConfigurator(mapConfigurator.GetInstructionConfiguration());
    //    ConsoleView.GetInstance().SetGame(this);

    //    foreach (var enemy in _room.Enemies)
    //        enemy.OwnDeath += EnemyDeathHandler;
    //    // View must access the model state
    //}
}

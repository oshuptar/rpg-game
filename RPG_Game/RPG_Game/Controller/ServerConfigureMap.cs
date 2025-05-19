using RPG_Game.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ServerConfigureMap : ServerState
{
    public ServerConfigureMap(GameServer gameServer) : base(gameServer) { }
    public override void HostGame()
    {
        MapConfigurator map = gameServer.Map;
        map.CreateEmptyDungeon();
        map.FillDungeon();
        map.AddCentralRoom();
        map.AddChambers();
        map.AddPaths();
        map.PlaceItems();
        map.PlaceDecoratedWeapons();
        map.PlaceDecoratedItems();
        map.PlaceDecoratedItems();
        map.PlaceEnemies();
        map.PlacePotions();

        gameServer.ServerController.SetGameState(map.GetResult());
        //Console.WriteLine(map.GetResult().RenderMap());
        ServerHandlerChain.GetInstance().SetServerController(gameServer.ServerController);
        this.SetGameServerState();
    }
    public override void SetGameServerState()
    {
        gameServer.ServerState = new ServerRun(gameServer);
    }
}

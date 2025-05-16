using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class GameSession
{
    private GameClient GameClient { get; set; }
    private GameServer? GameServer { get; set; }
    public GameSession() { }
    public GameSession(GameClient gameClient, GameServer? gameServer)
    {
        GameClient = gameClient;
        GameServer = gameServer;
    }

    public void SetClient(GameClient gameClient) => GameClient = gameClient;
    public void SetServer(GameServer gameServer) => GameServer = gameServer;
}

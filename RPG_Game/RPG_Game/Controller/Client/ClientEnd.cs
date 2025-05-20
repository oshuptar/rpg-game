using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ClientEnd : ClientState
{
    public ClientEnd(GameClient gameClient) : base(gameClient){}
    public override void HostGame()
    {
        gameClient.Client.Close();
        gameClient.Client.Dispose();
        SetGameClientState();
    }
    public override void SetGameClientState()
    {
        Environment.Exit(0);
    }
}

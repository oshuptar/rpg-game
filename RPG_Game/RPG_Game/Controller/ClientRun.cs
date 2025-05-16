using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ClientRun : ClientState
{
    public ClientRun(GameClient client) : base(client) { }
    public override void HostGame()
    {
        // Main client loop
        SetGameClientState();
    }
    public override void SetGameClientState()
    {
        gameClient.ClientState = new ClientEnd(gameClient);
    }
}

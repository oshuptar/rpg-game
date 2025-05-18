using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ClientStart : ClientState
{
    public ClientStart(GameClient gameClient) : base(gameClient)
    {
    }
    public override void HostGame()
    {
        gameClient.ClientController.BuildChain();
        SetGameClientState();
    }
    public override void SetGameClientState()
    {
        gameClient.ClientState = new ClientConnect(gameClient);
    }
}


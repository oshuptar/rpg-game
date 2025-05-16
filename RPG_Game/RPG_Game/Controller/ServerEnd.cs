using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ServerEnd : ServerState
{
    public ServerEnd(GameServer gameServer) : base(gameServer) { }
    public override void HostGame()
    {
        throw new NotImplementedException();
    }

    public override void SetGameServerState()
    {
        throw new NotImplementedException();
    }
}

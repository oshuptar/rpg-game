using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public interface IServerState
{
    public void HostGame();
    public void SetGameServerState();
}

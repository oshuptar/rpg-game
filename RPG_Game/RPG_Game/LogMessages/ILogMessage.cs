using RPG_Game.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.LogMessages;

public interface ILogMessage
{
    public void Send();
    public void SetView(IServerView view);
}

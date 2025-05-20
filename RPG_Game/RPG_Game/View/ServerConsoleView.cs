using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.LogMessages;
using RPG_Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.View;

public class ServerConsoleView : ServerView
{
    public ServerConsoleView() : base(true)
    {
    }

    public override void ReadInput(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested) 
        {
            RequestType? requestType = GameInputHandler.TranslateRequest();
            if (requestType != null)
            {
                //new Request(
                //    requestType.Value,
                //    Controller)
                //    //GameState.PlayerId,
                //    //CurrentFocus,
                //    /*FocusOn*/).SendRequest();
            }
        }
    }
}

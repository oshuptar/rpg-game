using RPG_Game.Controller;
using RPG_Game.Enums;
using RPG_Game.UIHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.View;

// Divide the views into two different categories, move input and reading input into a separate interface
public interface IServerView : IView
{

}
public abstract class ServerView : View, IServerView
{
    protected ServerView(bool flag) : base(true)
    {
    }
    public override void ReadInput(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            RequestType? requestType = GameInputHandler.TranslateRequest();
            if (requestType != null)
            {
                new Request(
                    requestType.Value,
                    Controller).SendRequest();
            }
        }
    }
}

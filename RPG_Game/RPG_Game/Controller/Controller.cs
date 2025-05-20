using RPG_Game.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public abstract class Controller : IController
{
    protected BlockingCollection<IRequest> Requests = new BlockingCollection<IRequest>(new ConcurrentQueue<IRequest>());
    public abstract void HandleRequest(CancellationToken cancellationToken);
    public abstract void HandleResponse(IResponse response);
    public abstract void SendRequest(IRequest request);
    public abstract void SetViewController();
}

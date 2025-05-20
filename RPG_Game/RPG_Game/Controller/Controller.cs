using RPG_Game.Model;
using RPG_Game.UIHandlers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public abstract class Controller : IController
{
    //protected IGameState GameState { get; private set; }
    protected BlockingCollection<IRequest> Requests = new BlockingCollection<IRequest>(new ConcurrentQueue<IRequest>());
    public abstract void HandleRequest(CancellationToken cancellationToken);
    public abstract void HandleResponse(IResponse response);
    public abstract void SendRequest(IRequest request);
    public abstract void SetViewController();
    //public virtual GameState GetGameState()
    //{
    //    return Room.GetGameState();
    //}
    //protected View.View View { get; set; }
    //protected Room Room { get; set; }
    //public Controller(View.View view, Room room)
    //{
    //    View = view;
    //    Room = room;
    //}
    //public virtual void SetView(View.View view)
    //{
    //    View = view;
    //}
    //public virtual void SetGameState(Room room)
    //{
    //    Room = room;
    //}
    //public virtual void SetViewController()
    //{
    //    View.SetController(this);
    //}
    //public abstract void HandleRequest(CancellationToken cancellationToken)
    //{

    //}
    //public abstract void SendRequest(IRequest request);
    //public abstract void HandleResponse(IResponse response);
}

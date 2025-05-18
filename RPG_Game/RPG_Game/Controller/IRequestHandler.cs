using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Enums;
using RPG_Game.View;

namespace RPG_Game.Controller;

public interface ICanHandleRequest
{
    public bool CanHandleRequest(Request request);
}
public interface IRequestHandler : ICanHandleRequest
{
    public List<IViewCommand>? HandleRequest(Request request);
    public void SetNext(IRequestHandler? handler);
}
//public interface IServerRequestHandler : ICanHandleRequest
//{
//    public void HandleRequest(Request request);
//    public void SetNext(IServerRequestHandler? handler);
//}
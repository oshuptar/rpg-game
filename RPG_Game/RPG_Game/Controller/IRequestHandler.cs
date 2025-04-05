using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Enums;

namespace RPG_Game.Controller;

public interface IRequestHandler
{
    public void HandleRequest(ActionRequest request);
    public void SetNext(IRequestHandler? handler);
    public bool CanHandleRequest(ActionRequest request);
}

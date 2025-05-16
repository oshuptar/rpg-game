using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public interface IInputHandler
{
    public RequestType? TranslateRequest();
    public void DispatchRequest(ActionRequest request);
}

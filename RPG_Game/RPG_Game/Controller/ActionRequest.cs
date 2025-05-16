using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Enums;

namespace RPG_Game.Controller;

public class ActionRequest
{
    private Context Context;
    private RequestType ActionType;
    public ActionRequest(Context context, RequestType actionType)
    {
        Context = context;
        ActionType = actionType;
    }
    public Context GetContext() => Context;
    public RequestType GetActionRequestType() => ActionType;
}

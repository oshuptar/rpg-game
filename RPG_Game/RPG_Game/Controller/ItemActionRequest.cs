using RPG_Game.Enums;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ItemActionRequest : Request
{
    [JsonInclude]
    public List<Item> Items { get; set; } = new List<Item>();
    public ItemActionRequest(RequestType requestType,
        IController receiver,
        int playerId, int currentFocus, FocusType focusOn) : base(requestType, receiver, playerId, currentFocus, focusOn)
    {}
    public ItemActionRequest() : base() { }
    public static ItemActionRequest CreateItemActionRequest(Request request)
    {
        return new ItemActionRequest(request.RequestType,
            request.Receiver,
            request.PlayerId,
            request.CurrentFocus,
            request.FocusOn);
    }
}

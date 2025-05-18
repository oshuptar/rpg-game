using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

// These are value types, hence are copied
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$discriminator")]
[JsonDerivedType(typeof(Weapon), "ItemActionRequest")]
public class Request : IRequest
{
    [JsonIgnore]
    public IController Receiver { get; set; }
    [JsonInclude]
    public int CurrentFocus { get; protected set; }
    [JsonInclude]
    public FocusType FocusOn { get; protected set; }
    // GameState is attaced by controller
    [JsonIgnore]
    public IGameState GameState { get; set; }

    [JsonInclude]
    public RequestType RequestType { get; }
    [JsonInclude]
    public int PlayerId { get; }
    public Request(
        RequestType requestType,
        IController receiver,
        int playerId,
        int currentFocus,
        FocusType focusOn)
    {
        RequestType = requestType;
        Receiver = receiver;
        PlayerId = playerId;
        CurrentFocus = currentFocus;
        FocusOn = focusOn;
    }
    public virtual void SendRequest()
    {
        Receiver.SendRequest(this);
    }
}

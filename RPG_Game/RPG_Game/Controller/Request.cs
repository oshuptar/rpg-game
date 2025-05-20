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

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$discriminator")]
[JsonDerivedType(typeof(ItemActionRequest), "ItemActionRequest")]
public class Request : IRequest
{
    [JsonIgnore]
    public IController Receiver { get; set; }
    [JsonInclude]
    public int CurrentFocus { get; protected set; }
    [JsonInclude]
    public FocusType FocusOn { get; protected set; }
    // GameState is attached by controller
    [JsonIgnore]
    public IGameState GameState { get; set; }
    [JsonInclude]
    public RequestType RequestType { get; private set; }
    [JsonInclude]
    public int PlayerId { get; set; }
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
    public Request(RequestType requestType, IController receiver)
    {
        RequestType = requestType;
        Receiver = receiver;
    }
    public Request() { }
    public virtual void SendRequest()
    {
        Receiver.SendRequest(this);
    }
}

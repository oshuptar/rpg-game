using RPG_Game.Enums;
using RPG_Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public interface IResponse
{
    public void HandleResponse();
    public IGameState GetGameState();
    public int? GetPlayerId();
}

public class Response : IResponse
{
    [JsonIgnore]
    public IController Controller { get; set; }
    [JsonInclude]
    public GameState GameState { get; set; }
    [JsonInclude]
    public int? PlayerId { get; set; } = null;
    [JsonIgnore]
    public RequestType RequestType { get;  set; }
    public void HandleResponse()
    {
        Controller.HandleResponse(this);
    }
    public IGameState GetGameState()
    {
        return GameState;
    }
    public int? GetPlayerId() => PlayerId;
    public Response() { }
    public Response(
        RequestType requestType,
        int? playerId,
        GameState gameState)
    {
        RequestType = requestType;
        PlayerId = playerId;
        GameState = gameState;
    }

    public Response(Response response)
    {
        Controller = response.Controller;
        GameState = response.GameState;
        PlayerId = response.PlayerId;
        RequestType = response.RequestType;
    }
}
using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.Model;

public class GameState : IGameState
{
    [JsonInclude]
    public int PlayerId { get; set; }
    [JsonInclude]
    private RoomState RoomState { get; set; }
    public GameState() { }
    public GameState(RoomState roomState, int playerId)
    {
        RoomState = roomState;
        PlayerId = playerId;
    }
    public Player GetPlayer()
    {
        return RoomState.ActivePlayers[PlayerId];
    }
    public List<Entity> GetVisibleEntities()
    {
        // Cange to Bfd where visibility would depend on the position
        return RoomState.Entities;
    }
    public List<Item>? GetItems(Position pos)
    {
        return RoomState.Grid[pos.X][pos.Y].Items;
    }

    public StringBuilder RenderMap()
    {
        return RoomState.RenderMap();
    }
    public Room CreateRoom()
    {
        return new Room(this.RoomState, PlayerId);
    }
}

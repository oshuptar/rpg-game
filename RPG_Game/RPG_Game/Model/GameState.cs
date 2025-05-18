using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Model;

public class GameState : IGameState
{
    private int _playerId;
    public int PlayerId 
    {
        get => _playerId;
        set
        {
            PlayerStats = RoomState.ActivePlayers[value].GetEntityStats();
            _playerId = value;
        }
    }
    private RoomState RoomState { get; set; }
    private EntityStats PlayerStats { get; set; }
    public GameState(RoomState roomState)
    {
        RoomState = roomState;
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
        return RoomState.Grid[pos.X, pos.Y].Items;
    }

    public StringBuilder RenderMap()
    {
        return RoomState.RenderMap();
    }
}

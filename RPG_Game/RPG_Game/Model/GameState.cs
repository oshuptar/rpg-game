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
        // Cange to Bfs where visibility would depend on the position
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
    public AuthorityGameState CreateRoom()
    {
        return new AuthorityGameState(this.RoomState, PlayerId);
    }
    public GameState GetGameState()
    {
        return this;
    }
    public List<Entity> GetVisibleEnemies()
    {
        return RoomState.Entities;
    }
    public void LockReadBlock(Position position)
       => RoomState.LockReadBlock(position);
    public void LockWriteBlock(Position position)
        => RoomState.LockWriteBlock(position);
    public void UnlockReadBlock(Position position)
        => RoomState.UnlockReadBlock(position);
    public void UnlockWriteBlock(Position position)
        => RoomState.UnlockWriteBlock(position);
    public void LockReadState()
        => RoomState.StateLock.EnterReadLock();
    public void LockWriteState()
        => RoomState.StateLock.EnterWriteLock();
    public void UnlockReadState()
       => RoomState.StateLock.ExitReadLock();
    public void UnlockWriteState()
        => RoomState.StateLock.ExitWriteLock();
}

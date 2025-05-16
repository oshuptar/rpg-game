using RPG_Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Model;

public class GameState : IGameState
{
    protected RoomState RoomState { get; set; }
}

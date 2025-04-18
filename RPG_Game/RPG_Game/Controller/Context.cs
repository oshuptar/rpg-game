using RPG_Game.Entiities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

// Context provides means between COR and Game Session objects
public class Context
{
    //private Player Player { get; set; }
    //private Room Room { get; set; }

    private Game GameSession { get; set; }

    public Context(Game game)
    {
        GameSession = game;
    }

    public Game GetGame() => GameSession;
    public Player GetPlayer() => GameSession.GetPlayer();
    public Room GetRoom() => GameSession.GetRoom();

    //public Context(Player player, Room room, IItem? item = null)
    //{
    //    this.Player = player;
    //    this.Room = room;
    //}

}


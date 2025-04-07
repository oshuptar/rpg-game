using RPG_Game.Entiities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class Context
{
    private Player Player { get; set; }
    private Room Room { get; set; }
    public Context(Player player, Room room, IItem? item = null)
    {
        this.Player = player;
        this.Room = room;
    }
    public Player GetPlayer() => Player;
    public Room GetRoom() => Room;
}


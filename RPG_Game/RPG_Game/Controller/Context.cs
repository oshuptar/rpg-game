using RPG_Game.Entiities;
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

    public Context(Player player, Room room)
    {
        this.Player = player;
        this.Room = room;
    }
    public Player GetPlayer() => Player;
    public Room GetRoom() => Room;
}

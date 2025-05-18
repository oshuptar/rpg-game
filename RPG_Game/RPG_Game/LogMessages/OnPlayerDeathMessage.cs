using RPG_Game.Entiities;
using RPG_Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.LogMessages;

public class OnPlayerDeathMessage
{
    public Player Player;
    public OnPlayerDeathMessage(Player player)
    {
        Player = player;
    }
}

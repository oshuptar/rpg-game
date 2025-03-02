using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public class Gold : ICurrency
{
    public string Name => "Gold";
    public int Value => 50;
    public void PickUp(Player player)
    {
        player.PickUp(this);
        player.AddGold(this);
    }
}

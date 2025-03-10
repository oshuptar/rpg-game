using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Currencies;

public class Coin : ICurrency
{
    public string Name => "Coin";
    public static int Value => 10;
    public void ApplyChanges(PlayerStats playerStats) => playerStats.ModifyPlayerAttribute("Coins", 1);
    public void RevertChanges(PlayerStats playerStats) => playerStats.ModifyPlayerAttribute("Coins", -1);
}

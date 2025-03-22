using RPG_Game.Entities;
using RPG_Game.Interfaces;
using RPG_Game.Entiities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Potions;

// Implement via events in PlayerStats class
public class AggressionPotion : IPotion
{
    public string Name => "Aggresion Potion";

    public int Aggression = 5;
    public void ApplyChanges(PlayerStats playerStats) 
    {
        playerStats.ModifyPlayerAttribute("Aggression", Aggression);
    }
    public void RevertChanges(PlayerStats playerStats)
    {
        playerStats.ModifyPlayerAttribute("Health", -Aggression);
    }

    public void Use(Player? player)
    {
        ApplyChanges(player?.RetrievePlayerStats());
    }

    public void Dispose(Player? player)
    {
        RevertChanges(player?.RetrievePlayerStats());
    }
}

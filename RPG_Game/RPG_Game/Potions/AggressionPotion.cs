using RPG_Game.Entities;
using RPG_Game.Interfaces;
using RPG_Game.Entiities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Enums;

namespace RPG_Game.Potions;

// Implement via events in PlayerStats class
public class AggressionPotion : IPotion
{
    public string Name => "Aggresion Potion";

    public int Aggression = 5;
    private void ApplyEffect(PlayerStats playerStats) 
    {
        playerStats.ModifyPlayerAttribute(PlayerAttributes.Aggression, Aggression);
    }
    //Made it private, so only possible to apply when used
    private void RevertEffect(PlayerStats playerStats) // ask about this private accessor
    {
        playerStats.ModifyPlayerAttribute(PlayerAttributes.Aggression, -Aggression);
    }

    public string Description => $"(Adds {Aggression} Aggression)";

    public void Use(Player? player)
    {
        ApplyEffect(player?.RetrievePlayerStats()!);
    }

    public void Dispose(Player? player)
    {
        RevertEffect(player?.RetrievePlayerStats()!);
    }
}

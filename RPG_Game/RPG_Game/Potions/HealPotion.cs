using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Potions;

public class HealPotion : IPotion
{
    public string Name => "Healing Potion";

    public int HP = 5;
    private void ApplyChanges(PlayerStats playerStats)
    {
        playerStats.ModifyPlayerAttribute("Health", HP);
    }
    private void RevertChanges(PlayerStats playerStats)
    {
        playerStats.ModifyPlayerAttribute("Health", -HP);
    }

    public string Description => $"(Adds {HP} HP)";

    public void Use(Player? player)
    {
        ApplyChanges(player?.RetrievePlayerStats());
    }

    public void Dispose(Player? player)
    {
        RevertChanges(player?.RetrievePlayerStats());
    }
}

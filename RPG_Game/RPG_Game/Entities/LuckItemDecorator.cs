using RPG_Game.Entities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entiities;

public class LuckItemDecorator : ItemDecorator
{
    public LuckItemDecorator(IItem item) : base(item) { }
    public override string Name => "(Lucky) " + item.Name;
    public override void ApplyChanges(PlayerStats playerStats)
    {
        playerStats.ModifyPlayerAttribute("Luck", 2);
        item.ApplyChanges(playerStats);
    }
    public override void RevertChanges(PlayerStats playerStats)
    {
        playerStats.ModifyPlayerAttribute("Luck", -2); // the name is hardcoded, think about the way to fix this
        item.RevertChanges(playerStats);
    }

}
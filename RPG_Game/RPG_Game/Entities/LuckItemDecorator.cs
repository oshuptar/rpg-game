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
    public override string Name => item.Name + "(Increased Luck)";
    public override void ApplyChanges(Player player)
    {
        player.PlayerStats.ModifyPlayerAttribute("Luck", 2);
        item.ApplyChanges(player);
    }

    public override void RevertChanges(Player player)
    {
        player.PlayerStats.ModifyPlayerAttribute("Luck", -2); // the name is hardcoded, think about the way to fix this
        item.RevertChanges(player);
    }

}

//Issue with capaciity retrievall for weapons
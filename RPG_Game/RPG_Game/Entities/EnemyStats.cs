using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public class EnemyStats : EntityStats
{
    public EnemyStats()
    {

        // provide enum key 
        Attributes.Add(PlayerAttributes.Health, 1000);
        Attributes.Add(PlayerAttributes.Strength, 50);
        Attributes.Add(PlayerAttributes.Luck, 10);
        Attributes.Add(PlayerAttributes.Aggression, 10);
        Attributes.Add(PlayerAttributes.Dexterity, 1);
        Attributes.Add(PlayerAttributes.Coins, 0);
        Attributes.Add(PlayerAttributes.Gold, 0);
        Attributes.Add(PlayerAttributes.Money, 0);
        OnMoneyChange();
    }
}

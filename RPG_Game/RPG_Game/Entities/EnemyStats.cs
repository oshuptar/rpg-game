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
        Attributes.Add(PlayerAttributes.Health, new AttributeValue(1000, 1000));
        Attributes.Add(PlayerAttributes.Strength, new AttributeValue(50));
        Attributes.Add(PlayerAttributes.Luck, new AttributeValue(10));
        Attributes.Add(PlayerAttributes.Aggression, new AttributeValue(10));
        Attributes.Add(PlayerAttributes.Dexterity, new AttributeValue(0));
        Attributes.Add(PlayerAttributes.Coins, new AttributeValue(0));
        Attributes.Add(PlayerAttributes.Gold, new AttributeValue(0));
        Attributes.Add(PlayerAttributes.Money, new AttributeValue(0));
        OnMoneyChange();
    }
}

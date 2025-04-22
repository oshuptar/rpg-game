using RPG_Game.Currencies;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

// Luck defines the chance of gving a critical damage which is calculated as 2*Damage
public class PlayerStats : EntityStats
{
    public PlayerStats()
    {
        Attributes.Add(PlayerAttributes.Health, new AttributeValue(1000, 1000));
        Attributes.Add(PlayerAttributes.Strength, new AttributeValue(0));
        Attributes.Add(PlayerAttributes.Luck, new AttributeValue(0, 100));
        Attributes.Add(PlayerAttributes.Aggression, new AttributeValue(0));
        Attributes.Add(PlayerAttributes.Dexterity, new AttributeValue(2));
        Attributes.Add(PlayerAttributes.Wisdom, new AttributeValue(0));
        Attributes.Add(PlayerAttributes.Coins, new AttributeValue(5));
        Attributes.Add(PlayerAttributes.Gold, new AttributeValue(0));
        Attributes.Add(PlayerAttributes.Money, new AttributeValue(0));
        OnMoneyChange();
    }

}

//public Dictionary<PlayerAttributes, int> Attributes { get; set; } = new Dictionary<PlayerAttributes, int>();

//Can we use reflections for retrieving the name of the attributes?
//public void ModifyPlayerAttribute(PlayerAttributes attribute, int value)
//{
//    if(Attributes.ContainsKey(attribute))
//        Attributes[attribute] += value;

//    if (attribute.Equals("Coins") || attribute.Equals("Gold"))
//        OnMoneyChange();
//}

//public void OnMoneyChange()
//{
//    Attributes[PlayerAttributes.Money] = Attributes[PlayerAttributes.Coins] * Coin.Value + Attributes[PlayerAttributes.Gold] * Gold.Value;
//}

//public (PlayerAttributes attribute, int value)? RetrievePlayerAttribute(PlayerAttributes attribute)
//{
//    if (!Attributes.ContainsKey(attribute))
//        return null;
//    return (attribute, Attributes[attribute]); 
//}
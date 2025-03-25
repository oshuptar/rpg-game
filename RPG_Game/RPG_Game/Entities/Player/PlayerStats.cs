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

public class PlayerStats : EntityStats
{
    public PlayerStats()
    {
        // provide enum key 
        Attributes.Add(PlayerAttributes.Health, 100);
        Attributes.Add(PlayerAttributes.Strength, 0);
        Attributes.Add(PlayerAttributes.Luck, 0);
        Attributes.Add(PlayerAttributes.Aggression, 0);
        Attributes.Add(PlayerAttributes.Dexterity, 2);
        Attributes.Add(PlayerAttributes.Coins, 5);
        Attributes.Add(PlayerAttributes.Gold, 0);
        Attributes.Add(PlayerAttributes.Money, 0);
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
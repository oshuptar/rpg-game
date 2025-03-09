using RPG_Game.Currencies;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public class PlayerStats
{
    // How to encapsuate objects inside
    public Dictionary<string, int> Attributes { get; private set; } = new Dictionary<string, int>();
    public PlayerStats()
    {
        Attributes.Add("Health", 100);
        Attributes.Add("Strength", 0);
        Attributes.Add("Luck", 0);
        Attributes.Add("Aggression", 0);
        Attributes.Add("Dexterity", 2);
        Attributes.Add("Coins", 5);
        Attributes.Add("Gold", 0);
        Attributes.Add("Money", 0);
        OnMoneyChange();
    }

    //Can we use reflections for retrieving the name of the attributes?
    public void ModifyPlayerAttribute(string attribute, int value)
    {
        if(Attributes.ContainsKey(attribute))
            Attributes[attribute] += value;

        if (attribute.Equals("Coins") || attribute.Equals("Gold"))
            OnMoneyChange();
    }

    public void OnMoneyChange()
    {
        Attributes["Money"] = Attributes["Coins"] * Coin.Value + Attributes["Gold"] * Gold.Value;
    }

    public (string attribute, int value)? RetrievePlayerAttribute(string attribute)
    {
        if (!Attributes.ContainsKey(attribute))
            return null;
        return (attribute, Attributes[attribute]); 
    }
}


//public void AddCoin()
//{
//    CollectedCoins++;
//    TotalMoneyValue += Coin.Value;
//}
//public void AddGold()
//{
//    CollectedGold++;
//    TotalMoneyValue += Gold.Value;
//}
//public void RemoveCoin()
//{
//    CollectedCoins--;
//    TotalMoneyValue -= Coin.Value;
//}

//public void RemoveGold()
//{
//    Attributes--;
//    TotalMoneyValue -= Gold.Value;
//}

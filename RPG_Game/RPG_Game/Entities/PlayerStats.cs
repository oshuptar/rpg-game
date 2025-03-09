using RPG_Game.Currencies;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public class PlayerStats
{
    public Dictionary<string, int> Attributes { get; private set; } = new Dictionary<string, int>();
    public int TotalMoneyValue { get; private set; } = 50; // 50 as a default value
    public int CollectedCoins { get; private set; } = 0;
    public int CollectedGold { get; private set; } = 0;
    public PlayerStats()
    {
        Attributes.Add("Health", 100);
        Attributes.Add("Strength", 0);
        Attributes.Add("Luck", 0);
        Attributes.Add("Aggression", 0);
        Attributes.Add("Dexterity", 2);
    }
    public void AddCoin()
    {
        CollectedCoins++;
        TotalMoneyValue += Coin.Value;
    }
    public void AddGold()
    {
        CollectedGold++;
        TotalMoneyValue += Gold.Value;
    }
    public void RemoveCoin()
    {
        CollectedCoins--;
        TotalMoneyValue -= Coin.Value;
    }

    public void RemoveGold()
    {
        CollectedGold--;
        TotalMoneyValue -= Gold.Value;
    }
}

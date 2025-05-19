using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Currencies;

public abstract class Currency : Item
{
    //public static virtual int Value { get; }
    //public virtual string GetDescription() => $"A currency {Name} with value {Value}";
}

public class Coin : Currency
{
    public override string Name => "Coin";
    //[JsonInclude]
    public static int Value => 10;
    public override void ApplyChanges(EntityStats entityStats) => entityStats.ModifyEntityAttribute(PlayerAttributes.Coins, 1);
    public override void RevertChanges(EntityStats entityStats) => entityStats.ModifyEntityAttribute(PlayerAttributes.Coins, -1);
    public override string Description => "";
    public override object Copy() => new Coin();
}

public class Gold : Currency
{
    public override string Name => "Gold";
    //[JsonInclude]
    public static int Value => 50;
    public override void ApplyChanges(EntityStats entityStats) => entityStats.ModifyEntityAttribute(PlayerAttributes.Gold, 1);
    public override void RevertChanges(EntityStats entityStats) => entityStats.ModifyEntityAttribute(PlayerAttributes.Gold, -1);
    public override string Description => "";

    public override object Copy() => new Gold();
}


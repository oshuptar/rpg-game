using RPG_Game.Entiities;
using RPG_Game.Interfaces;
using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Entities;
using System.Text.Json.Serialization;

namespace RPG_Game.Currencies;

public class Coin : ICurrency
{
    public override string Name => "Coin";
    [JsonInclude]
    public static int Value => 10;
    public override void ApplyChanges(EntityStats entityStats) => entityStats.ModifyEntityAttribute(PlayerAttributes.Coins, 1);
    public override void RevertChanges(EntityStats entityStats) => entityStats.ModifyEntityAttribute(PlayerAttributes.Coins, -1);
    public override string Description => "";
    public override object Copy() => new Coin();
}

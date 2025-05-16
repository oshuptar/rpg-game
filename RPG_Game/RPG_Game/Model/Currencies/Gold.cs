using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Interfaces;
using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Currencies;

public class Gold : ICurrency
{
    public override string Name => "Gold";
    public static int Value => 50;
    public override void ApplyChanges(EntityStats entityStats) => entityStats.ModifyEntityAttribute(PlayerAttributes.Gold, 1);
    public override void RevertChanges(EntityStats entityStats) => entityStats.ModifyEntityAttribute(PlayerAttributes.Gold, -1);

    public override string Description => "";

    public override object Copy() => new Gold();
}

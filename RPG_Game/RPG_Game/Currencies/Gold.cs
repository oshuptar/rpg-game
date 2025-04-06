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
    public string Name => "Gold";
    public static int Value => 50;
    public void ApplyChanges(EntityStats entityStats) => entityStats.ModifyEntityAttribute(PlayerAttributes.Gold, 1);
    public void RevertChanges(EntityStats entityStats) => entityStats.ModifyEntityAttribute(PlayerAttributes.Gold, -1);

    public string Description => "";
}

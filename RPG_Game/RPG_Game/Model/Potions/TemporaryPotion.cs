using RPG_Game.Entities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.Potions;
public abstract class TemporaryPotion : Potion
{
    [JsonInclude]
    public abstract int ActiveTime { get; protected set; }
    public abstract int Lifetime { get; }
    public abstract void OnMoveHandler(object sender, EventArgs e);
    public abstract void RevertEffect(EntityStats? entityStats);
}

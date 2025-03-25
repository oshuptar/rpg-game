using RPG_Game.Currencies;
using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RPG_Game.Entities;

//Marking the class as abstract
public abstract class EntityStats
{
    public virtual Dictionary<PlayerAttributes, int> Attributes { get; private set; } = new Dictionary<PlayerAttributes, int>();
    public virtual void ModifyPlayerAttribute(PlayerAttributes attribute, int value)
    {
        if (Attributes.ContainsKey(attribute))
            Attributes[attribute] += value;

        if (attribute.Equals("Coins") || attribute.Equals("Gold"))
            OnMoneyChange();
    }

    public virtual void OnMoneyChange()
    {
        Attributes[PlayerAttributes.Money] = Attributes[PlayerAttributes.Coins] * Coin.Value + Attributes[PlayerAttributes.Gold] * Gold.Value;
    }

    public virtual (PlayerAttributes attribute, int value)? RetrievePlayerAttribute(PlayerAttributes attribute)
    {
        if (!Attributes.ContainsKey(attribute))
            return null;
        return (attribute, Attributes[attribute]);
    }
}

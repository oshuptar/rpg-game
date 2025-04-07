using RPG_Game.Currencies;
using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RPG_Game.Entities;

public class AttributeValue
{
    private int Current;
    private int? Max;

    public AttributeValue(int current, int? max = null)
    {
        this.Current = current;
        this.Max = max;
    }

    public int GetCurrentValue() => this.Current;

    public void ModifyAttributeValue(int addedValue, int addedMax = 0)
    {
        this.Max += addedMax; // operations that involve null are handled automatically
        if (this.Max != null)
            this.Current = (this.Current + addedValue) >= this.Max.Value ? this.Max.Value : this.Current + addedValue;
        else
            this.Current += addedValue;
    }

    public void SetCurrentValue(int currentValue)
    {
        this.Current = currentValue;
    }

    public override string ToString() => this.Current.ToString();
}


public abstract class EntityStats
{
    // It is good to define maximum value in dictionary for each attribute!
    // This will allow for example to change upper bound of Health if you drink Strength potion
    public virtual Dictionary<PlayerAttributes, AttributeValue> Attributes { get; private set; } = 
        new Dictionary<PlayerAttributes, AttributeValue>();

    public virtual void ModifyEntityAttribute(PlayerAttributes attribute, int currentValue, int maxValue = 0)
    {
        if(Attributes.ContainsKey(attribute)) 
            Attributes[attribute].ModifyAttributeValue(currentValue, maxValue);

        if (attribute == PlayerAttributes.Coins || attribute == PlayerAttributes.Gold)
            OnMoneyChange();
    }

    public virtual void OnMoneyChange()
    {
        Attributes[PlayerAttributes.Money].SetCurrentValue(Attributes[PlayerAttributes.Coins].GetCurrentValue() * Coin.Value 
            + Attributes[PlayerAttributes.Gold].GetCurrentValue() * Gold.Value);
    }

    public virtual (PlayerAttributes attribute, AttributeValue attributeValue)? RetrieveEntityAttribute(PlayerAttributes attribute)
    {
        if (!Attributes.ContainsKey(attribute))
            return null;
        return (attribute, Attributes[attribute]);
    }
}

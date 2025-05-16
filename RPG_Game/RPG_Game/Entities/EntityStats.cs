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
    public void ModifyAttributeValue(int addedValue, int addedMax = 0)
    {
        this.Max += addedMax; // operations that involve null are handled automatically
        if (addedValue > 0)
        {
            if (this.Max != null)
                this.Current = Math.Min(this.Max.Value, this.Current + addedValue);
            else
                this.Current += addedValue;
        }
        else if(addedValue <= 0)
        {
            this.Current = Math.Max(0, this.Current + addedValue); // would case a problem with money, be aware
        }
    }
    public int GetCurrentValue() => this.Current;
    public void SetCurrentValue(int currentValue) => this.Current = currentValue;
    public override string ToString() => this.Current.ToString();
}
public abstract class EntityStats
{
    // It is good to define maximum value in dictionary for each attribute!
    // This will allow for example to change upper bound of Health if you drink Strength potion
    public event EventHandler? Died;
    public virtual Dictionary<PlayerAttributes, AttributeValue> Attributes { get; private set; } =
        new Dictionary<PlayerAttributes, AttributeValue>();
    public virtual void ModifyEntityAttribute(PlayerAttributes attribute, int currentValue, int maxValue = 0)
    {
        if (Attributes.ContainsKey(attribute))
            Attributes[attribute].ModifyAttributeValue(currentValue, maxValue);

        if (attribute == PlayerAttributes.Coins || attribute == PlayerAttributes.Gold)
            OnMoneyChange();

        //Posts an event to listener about its death ( no need to perform checks after each change of attribute )
        if (attribute == PlayerAttributes.Health && Attributes[attribute].GetCurrentValue() <= 0)
            OnDeath();
    }
    protected virtual void OnDeath()
    {
        Died?.Invoke(this, EventArgs.Empty);
    }
    protected virtual void OnMoneyChange()
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

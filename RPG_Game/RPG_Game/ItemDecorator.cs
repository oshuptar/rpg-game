using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public class ItemDecorator : IItem
{
    protected IItem item;
    public virtual string Name => item.Name;
    //public override string ToString() => item.ToString();
    public ItemDecorator(IItem item)
    {
        this.item = item;
    }
}

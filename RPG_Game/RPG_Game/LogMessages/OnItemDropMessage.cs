using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.LogMessages;

public class OnItemDropMessage : OnItemActionMessage
{
    public OnItemDropMessage(IItem item, string name)
    {
        this.Item = item;
        this.Name = name;
    }
}

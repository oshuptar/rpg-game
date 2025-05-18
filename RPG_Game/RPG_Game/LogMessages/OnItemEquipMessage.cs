using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.LogMessages;

public class OnItemEquipMessage :OnItemActionMessage
{
    public OnItemEquipMessage(Item item, string name) : base(item, name)
    {
    }
}

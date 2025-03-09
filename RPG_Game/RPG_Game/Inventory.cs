using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public class Inventory
{
    public List<IItem> inventory { get; } = new List<IItem>();
}

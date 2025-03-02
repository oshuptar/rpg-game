using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

// Do items need to contain attributes of a Player?
public interface IItem
{
    public string Name { get; }
}

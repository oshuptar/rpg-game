using RPG_Game.Entiities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;

// Categorizing interface
public interface IPotion : IItem
{
    public void Dispose(Player player);
}

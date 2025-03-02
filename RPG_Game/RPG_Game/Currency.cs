using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public interface ICurrency : IItem
{
    public int Value { get; }
    public string GetDescription() => $"A currency {Name} with value {Value}";
}

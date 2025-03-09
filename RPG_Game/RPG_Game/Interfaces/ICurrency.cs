using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;

public interface ICurrency : IItem
{
    public static int Value { get; }
    public string GetDescription() => $"A currency {Name} with value {Value}";
}

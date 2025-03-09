using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entiities;

public class LuckItemDecorator : ItemDecorator
{
    public LuckItemDecorator(IItem item) : base(item) { }
    public override string Name => item.Name + "(Increased Luck)";
}

//Issue with capaciity retrievall for weapons
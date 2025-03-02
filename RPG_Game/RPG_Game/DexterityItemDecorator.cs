using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public class DexterityItemDecorator : ItemDecorator
{
    public DexterityItemDecorator(IItem item) : base(item) { }
    public override string Name => item.Name + "(Decreased Dexterity)";
}

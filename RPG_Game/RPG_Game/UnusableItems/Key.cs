using RPG_Game.Entiities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using RPG_Game.Entities;

namespace RPG_Game.UnusableItems;

public class Key : ILoot
{
    public override string Name => "Key";
    //public override void Use(IEntity? entity = null) { }

    public override string Description => $"(This is a old rusty key. Maybe it opens a chest...?)";

    public override object Copy() => new Key();
}

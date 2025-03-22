using RPG_Game.Entiities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RPG_Game.UnusableItems;

public class Key : ILoot
{
    public string Name => "Key";
    public void Use(Player? player = null) { }

    public string Description => $"(This is a old rusty key. Maybe it opens a chest...?)";
}

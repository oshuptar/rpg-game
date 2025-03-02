using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RPG_Game;

public class Key : ILoot
{
    public string Name => "Key";
    public void Inspect() => Console.WriteLine("This is a old rusty key. Maybe it opens a chest...?");
}

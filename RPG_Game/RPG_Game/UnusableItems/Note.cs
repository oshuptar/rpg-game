using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RPG_Game.UnusableItems;

public class Note : ILoot
{
    public string Name => "A mysterious note";
    public void Inspect() => Console.WriteLine("Hmm.. Exploration - is the KEY to finding treasures");
}

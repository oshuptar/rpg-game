using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.UnusableItems;

public class Lore : ILoot
{
    public string Name => "Lore";

    // Possible use of Cursor.SetPosition for a nice output
    public void Inspect() => Console.WriteLine("In the distant past, the land was ruled by Shogun Ryūjin, a warrior of unmatched skill and a ruler with an iron will.\n" +
        " He unified the warring clans under his banner, bringing an age of prosperity—but also fear.\n" +
        " Ryūjin was obsessed with power and believed that wealth and mystical relics would grant him eternal rule.\n\n" +
        "He sent his most loyal samurai across the land to gather the greatest treasures ever known—artifacts of gods, weapons of war, and riches beyond imagination.\n" +
        " But before he could wield his newfound power, he mysteriously vanished, along with his hidden treasury.\n" +
        " Some say he was betrayed by his own generals, others believe he performed a forbidden ritual and became something more than human.\n\n" +
        "Now, centuries later, his hidden treasures remain scattered across the land, locked away in ancient temples, buried beneath fallen castles, or guarded by supernatural forces.\n" +
        " Those who seek them must face the dangers of Ryūjin’s wrath—for his spirit is said to still wander, watching over his legacy.");

}

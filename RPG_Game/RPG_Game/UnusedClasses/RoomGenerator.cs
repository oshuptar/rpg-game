using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.Weapons;
using RPG_Game.Currencies;
using RPG_Game.UnusableItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.HelperClasses;

public static class RoomGenerator
{
    // to be improved
    private static List<IItem> ItemList = new List<IItem>()
    {
        new Hammer(),
        new Sword(),
        new LuckItemDecorator(new Sword()),
        new Dagger(),
        new PowerWeaponDecorator(new Hammer()),
        new PowerWeaponDecorator(new PowerWeaponDecorator(new Hammer())),
        new LuckItemDecorator(new Gold()),
        new Gold(),
        new Coin(),
        new LuckItemDecorator( new PowerWeaponDecorator(new PowerWeaponDecorator(new Hammer()))),
        new Key(),
        new LuckItemDecorator(new Key()),
        new Note(),
        new Lore()
    };
    //new PowerWeaponDecorator(new LuckItemDecorator(new Dagger())), - think what to do with something like that
    public static void RandomRoomGeneration(Room room)
    {
        // const objects are implicitly static
        //10% of obstacles of the total map size
        int widthPlayAreaSize = Room._width - 2 * Room._frameSize;
        int heightPlayAreaSize = Room._height - 2 * Room._frameSize;
        Random random = new Random();
        for (int i = 0; i < widthPlayAreaSize * heightPlayAreaSize / 5; i++)
        {
            int X = Room._frameSize + random.Next() % (widthPlayAreaSize - Room._frameSize);
            int Y = Room._frameSize + random.Next() % (heightPlayAreaSize - Room._frameSize);
            room.AddObject(CellType.Wall, (X, Y));
        }

        int randomIndex;
        // %20 filled with different items
        for (int i = 0; i < widthPlayAreaSize * heightPlayAreaSize / 8; i++)
        {
            int X = Room._frameSize + random.Next() % (widthPlayAreaSize - Room._frameSize);
            int Y = Room._frameSize + random.Next() % (heightPlayAreaSize - Room._frameSize);

            randomIndex = random.Next() % ItemList.Count;
            room.AddItem(ItemList[randomIndex], (X, Y));
        }
    }
}

// Randomize two points on the map and try to connect them to generate paths
// The starting position does not need to be the 0,0 point
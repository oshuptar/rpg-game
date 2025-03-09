using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public static class RoomGenerator
{
    // to be improved
    private static List<IItem> ItemList = new List<IItem>()
    {
        new Hammer(),
        new Sword(),
        new LuckItemDecorator(new Sword()),
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

    public static void RandomRoomGeneration(Room room)
    {
        // const objects are implicitly static
        //10% of obstacles of the total map size
        int widthPlayAreaSize = (Room._width - 2 * Room._frameSize);
        int heightPlayAreaSize = (Room._height - 2 * Room._frameSize);
        Random random = new Random();
        for (int i = 0; i < widthPlayAreaSize * heightPlayAreaSize / 10; i++)
        {
            int X = Room._frameSize + random.Next() % (widthPlayAreaSize - Room._frameSize);
            int Y = Room._frameSize + random.Next() % (heightPlayAreaSize - Room._frameSize);
            room.AddObject(CellType.Wall, (X, Y));
        }

        int randomIndex;
        // %20 filled with different items
        for (int i = 0; i < widthPlayAreaSize * heightPlayAreaSize / 5; i++)
        {
            int X = Room._frameSize + random.Next() % (widthPlayAreaSize - Room._frameSize);
            int Y = Room._frameSize + random.Next() % (heightPlayAreaSize - Room._frameSize);

            randomIndex = random.Next() % ItemList.Count;
            room.AddItem(ItemList[randomIndex], (X, Y));
        }
    }
}

//IWeapon item1 = new Hammer();
//IWeapon item2 = new PowerWeaponDecorator(item1);
//IWeapon item3 = new PowerWeaponDecorator(item2);
//IItem item4 = new DexterityItemDecorator(item3);
//IItem item5 = new Coin();
//IItem item6 = new Gold();

//List<IItem> tempItems = new List<IItem> { item1, item2, item3, item4 };

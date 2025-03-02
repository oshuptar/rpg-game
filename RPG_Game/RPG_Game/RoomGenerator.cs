using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public static class RoomGenerator
{
    // to be improved
    public static void RandomRoomGeneration(Room room)
    {
        // const objects are implicitly static

        //Let's say we would have 10% of obstacles of the total map size
        int widthPlayAreaSize = (Room._width - 2 * Room._frameSize);
        int heightPlayAreaSize = (Room._height - 2 * Room._frameSize);
        Random random = new Random();
        for (int i = 0; i < widthPlayAreaSize * heightPlayAreaSize / 10; i++)
        {
            int X = Room._frameSize + random.Next() % (widthPlayAreaSize - Room._frameSize);
            int Y = Room._frameSize + random.Next() % (heightPlayAreaSize - Room._frameSize);
            room.AddObject(CellType.Wall, (X, Y));
        }

        IWeapon item1 = new Hammer();
        IWeapon item2 = new PowerWeaponDecorator(item1);
        IWeapon item3 = new PowerWeaponDecorator(item2);
        IItem item4 = new DexterityItemDecorator(item3);

        List<IItem> tempItems = new List<IItem> { new Coin(), new Gold(), item1, item2, item3, item4 };
        int randomIndex;
        // %20 filled with different items
        for (int i = 0; i < widthPlayAreaSize * heightPlayAreaSize / 5; i++)
        {
            int X = Room._frameSize + random.Next() % (widthPlayAreaSize - Room._frameSize);
            int Y = Room._frameSize + random.Next() % (heightPlayAreaSize - Room._frameSize);

            randomIndex = random.Next() % tempItems.Count;
            room.AddItem(tempItems[randomIndex], (X, Y));
        }
    }
}

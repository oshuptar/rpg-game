using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RPG_Game;

public static class ObjectDisplayer
{
    static int CurrentFocus = 0;
    public static void DisplayTileItems(Room room, (int x, int y) position)
    {
        string? output = null;
        if (room.Items[position.x, position.y] != null)
        {
            // On each element of the sequence the lambda function is called
            output = String.Join(',', room.Items[position.x, position.y].Select(item => item.Name));
        }
        // Displays none if output == null
        Console.WriteLine($"Items: {output ?? "None"}");
    }

    public static void DisplayCurrent(Room room, (int x, int y) position)
    {
        string? output = null;
        if(room.Items[position.x, position.y] != null)
        {
            output = room.Items[position.x, position.y][CurrentFocus].Name;
        }

        Console.WriteLine($"Current Focus : {output ?? "None"}");
    }

    public static void IncreaseCurrentFocus(Room room, (int x, int y) position)
    {
        CurrentFocus = (CurrentFocus + 1 <= room.Items[position.x, position.y].Count - 1) ? CurrentFocus + 1 : CurrentFocus;
    }

    public static void DecreaseCurrentFocus(Room room, (int x, int y) position)
    {
        CurrentFocus = ((CurrentFocus - 1) >= 0) ? CurrentFocus - 1 : CurrentFocus;
    }


    public static void ResetFocus()
    {
        CurrentFocus = 0;
    }
}

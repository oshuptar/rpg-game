using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RPG_Game;

public static class ObjectDisplayer
{
    public static int CurrentFocus = 0;
    public static void DisplayTileItems(Room room, (int x, int y) position)
    {
        string? output = null;
        if (room.Items[position.x, position.y] != null && room.Items[position.x, position.y].Count != 0)
        {
            // On each element of the sequence the lambda function is called
            output = String.Join(',', room.Items[position.x, position.y].Select(item => item.Name));
        }
        // Displays none if output == null
        Console.WriteLine($"Items: {output ?? "None"}");
    }

    public static void PrintGrid(Room room)
    {
        for (int i = 0; i < Room._height; i++)
        {
            for (int j = 0; j < Room._width; j++)
            {
                room.Grid[j, i].PrintCell();
            }
            Console.WriteLine();
        }
    }

    public static void DisplayCurrent(Room room, (int x, int y) position)
    {
        string? output = null;
        if(room.Items[position.x, position.y] != null && room.Items[position.x, position.y].Count != 0)
        {
            output = room.Items[position.x, position.y][CurrentFocus].Name;
        }
        Console.WriteLine($"Current Focus : {output ?? "None"}");
    }

    public static void IncreaseCurrentFocus(Room room, (int x, int y) position)
    {
        if (room.Items[position.x, position.y] is not null)
            CurrentFocus = (CurrentFocus + 1 <= room.Items[position.x, position.y].Count - 1) ? CurrentFocus + 1 : CurrentFocus;
    }

    public static void DecreaseCurrentFocus(Room room, (int x, int y) position)
    {
        if (room.Items[position.x, position.y] is not null)
            CurrentFocus = ((CurrentFocus - 1) >= 0) ? CurrentFocus - 1 : CurrentFocus;
    }

    public static void ResetFocus()
    {
        CurrentFocus = 0;
    }


    public static void DisplayInventory(Player player)
    {
        string? output = null;

        if (player.inventory != null && player.inventory.Count != 0)
            output = String.Join(',', player.inventory.Select(item => item.Name));

        Console.WriteLine($"Inventory of the player: {output ?? "None"}");
    }

    public static void DisplayRoutine()
    {

    }
}


//public void PrintGrid(Room room)
//{
//    for (int i = 0; i < Room._height; i++)
//    {
//        for (int j = 0; j < Room._width; j++)
//        {
//            room.Grid[j, i].PrintCell();
//        }
//        Console.WriteLine();
//    }
//}
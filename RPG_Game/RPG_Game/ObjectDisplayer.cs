using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RPG_Game;

public static class ObjectDisplayer
{
    public static int CurrentFocus = 0;

    public static void DisplayItemList(List<IItem> list, string name)
    {
        string? output = null;
        if (list != null && list.Count != 0)
        {
            // On each element of the sequence the lambda function is called
            output = String.Join(", ", list.Select(item => item.Name));
        }
        // Displays none if output == null
        Console.Write($"{name}: {output ?? "None"}\n");
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
    public static void DisplayTileItems(Room room, (int x, int y) position)
    {
        DisplayItemList(room.Items[position.x, position.y], "Items");
    }

    public static void DisplayCurrent(Room room, (int x, int y) position)
    {
        string? output = null;
        if (room.Items[position.x, position.y] != null && room.Items[position.x, position.y].Count != 0)
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
        DisplayItemList(player.inventory, "Inventory");
    }

    public static void DisplayEquipped(Player player)
    {
        DisplayItemList(player.hands, "Equipped");
    }

    public static void DisplayRoutine(Room _room, Player player)
    {

        ObjectDisplayer.PrintGrid(_room); // Print the grid

        int spaceSize = 10;
        int horizontalPosition = Room._width + spaceSize;
        int verticalPosition = Room._height / 16;

        (int X, int Y) oldPosition = Console.GetCursorPosition();
        //Fix Output; Consider doing it with events
        Console.SetCursorPosition(horizontalPosition, verticalPosition);
        ObjectDisplayer.DisplayTileItems(_room, player.Position);
        Console.SetCursorPosition(horizontalPosition, verticalPosition + 1);
        ObjectDisplayer.DisplayCurrent(_room, player.Position);
        Console.SetCursorPosition(horizontalPosition, verticalPosition + 2);
        ObjectDisplayer.DisplayInventory(player);
        Console.SetCursorPosition(horizontalPosition, verticalPosition + 3);
        ObjectDisplayer.DisplayEquipped(player);

        Console.SetCursorPosition(oldPosition.X, oldPosition.Y);
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
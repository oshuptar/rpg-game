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
    public static int CurrentFocus { get; private set; } = 0;
    public static FocusType FocusOn { get; private set; } = FocusType.Room;
    private static (int left, int top) CursorPosition;
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

    public static void DisplayCurrent(List<IItem>? list, string Object)
    {
        string? output = null;
        if (list != null && list.Count != 0)
        {
            output = list[CurrentFocus].Name;
        }
        Console.WriteLine($"Current Focus (in {Object}): {output ?? "None"}");
    }

    public static void DisplayCurrentItem(Room room, Player player)
    {

        switch (FocusOn)
        {
            case FocusType.Inventory:
                DisplayCurrent(player.inventory, "Inventory");
                break;
            case FocusType.Hands:
                DisplayCurrent(player.hands, "Hands");
                break;
            case FocusType.Room:
                DisplayCurrent(room.Items[player.Position.x, player.Position.y], "Room");
                break;
        }
    }

    //Possibillity to navigate through inventory in 4 directions later on
    public static void ShiftFocus(List<IItem>? list, Direction direction)
    {
        if (list is null)
            return;

        switch(direction)
        {
            case Direction.Left:
                CurrentFocus = ((CurrentFocus - 1) >= 0) ? CurrentFocus - 1 : CurrentFocus;
                break;
            case Direction.Right:
                CurrentFocus = (CurrentFocus + 1 <= list!.Count - 1) ? CurrentFocus + 1 : CurrentFocus;
                break;
        }
    }

    public static void ShiftCurrentFocus(Room room, Player player, Direction direction)
    {
        switch(FocusOn)
        {
            case FocusType.Inventory:
                ShiftFocus(player.inventory, direction);
                break;
            case FocusType.Hands:
                ShiftFocus(player.hands, direction);
                break;
            case FocusType.Room:
                ShiftFocus(room.Items[player.Position.x, player.Position.y], direction);
                break;
        }
    }
    public static void DisplayInventory(Player player)
    {
        DisplayItemList(player.inventory, "Inventory");
    }

    public static void DisplayEquipped(Player player)
    {
        DisplayItemList(player.hands, "Equipped");
    }

    public static void ResetFocusIndex() => CurrentFocus = 0;
    public static void SetInventoryFocus() => FocusOn = FocusType.Inventory;
    public static void SetHandsFocus() => FocusOn = FocusType.Hands;
    public static void ResetFocusType() => FocusOn = FocusType.Room;

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
        ObjectDisplayer.DisplayEquipped(player);
        Console.SetCursorPosition(horizontalPosition, verticalPosition + 2);
        ObjectDisplayer.DisplayInventory(player);
        Console.SetCursorPosition(horizontalPosition, verticalPosition + 3);
        ObjectDisplayer.DisplayCurrentItem(_room, player);

        Console.SetCursorPosition(oldPosition.X, oldPosition.Y);
    }
}
using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.UnusableItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RPG_Game.HelperClasses;

public static class ObjectDisplayer
{
    public static int CurrentFocus { get; private set; } = 0;
    public static FocusType FocusOn { get; private set; } = FocusType.Room;
    private static (int left, int top) CursorPosition { get; set; }
    private static bool IsControlsVisible { get; set; } = true;
    public static void DisplayItemList(List<IItem> list, string name)
    {
        string? output = null;
        if (list != null && list.Count != 0)
        {
            // On each element of the sequence the lambda function is called
            output = string.Join(", ", list.Select(item => item.Name));
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

        Console.Write($"Current Focus (in {Object}): ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{ output ?? "None"}");
        Console.WriteLine();
        Console.ResetColor();
    }

    public static void DisplayCurrentItem(Room room, Player player)
    {

        switch (FocusOn)
        {
            case FocusType.Inventory:
                DisplayCurrent(player.RetrieveInventory(), "Inventory");
                break;
            case FocusType.Hands:
                DisplayCurrent(player.RetrieveHands(), "Hands");
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
                CurrentFocus = CurrentFocus - 1 >= 0 ? CurrentFocus - 1 : CurrentFocus;
                break;
            case Direction.Right:
                CurrentFocus = CurrentFocus + 1 <= list!.Count - 1 ? CurrentFocus + 1 : CurrentFocus;
                break;
        }
    }

    public static void ShiftCurrentFocus(Room room, Player player, Direction direction)
    {
        switch(FocusOn)
        {
            case FocusType.Inventory:
                ShiftFocus(player.RetrieveInventory(), direction);
                break;
            case FocusType.Hands:
                ShiftFocus(player.RetrieveHands(), direction);
                break;
            case FocusType.Room:
                ShiftFocus(room.Items[player.Position.x, player.Position.y], direction);
                break;
        }
    }
    public static void DisplayInventory(Player player)
    {
        DisplayItemList(player.RetrieveInventory(), "Inventory");
    }

    public static void DisplayEquipped(Player player)
    {
        DisplayItemList(player.RetrieveHands(), "Equipped");
    }

    public static void DisplayPlayerAttributes(Player player)
    {
        foreach(var key in player.PlayerStats.Attributes.Keys)
        {
            Console.Write($"{key}: {player.PlayerStats.Attributes[key]}");
            CursorPosition = (CursorPosition.left, CursorPosition.top + 1);
            Console.SetCursorPosition(CursorPosition.left, CursorPosition.top);
        }
    }

    public static void ResetFocusIndex() => CurrentFocus = 0;
    public static void SetInventoryFocus() => FocusOn = FocusType.Inventory;
    public static void SetHandsFocus() => FocusOn = FocusType.Hands;
    public static void ResetFocusType() => FocusOn = FocusType.Room;

    public static void DisplayRoutine(Room _room, Player player)
    {

        PrintGrid(_room); // Print the grid

        int spaceSize = 10;
        int horizontalPosition = Room._width + spaceSize;
        int verticalPosition = Room._height / 16;

        (int X, int Y) oldPosition = Console.GetCursorPosition();
        //Fix Output; Consider doing it with events
        Console.SetCursorPosition(horizontalPosition, verticalPosition);
        DisplayTileItems(_room, player.Position);
        Console.SetCursorPosition(horizontalPosition, verticalPosition + 1);
        DisplayEquipped(player);
        Console.SetCursorPosition(horizontalPosition, verticalPosition + 2);
        DisplayInventory(player);
        Console.SetCursorPosition(horizontalPosition, verticalPosition + 3);
        DisplayCurrentItem(_room, player);

        CursorPosition = (horizontalPosition, verticalPosition + 3 + Room._height / 16);
        Console.SetCursorPosition(CursorPosition.left,CursorPosition.top);
        DisplayPlayerAttributes(player);

        //CursorPosition = (horizontalPosition, verticalPosition + 3 + Room._height / 2);
        //Console.SetCursorPosition(CursorPosition.left, CursorPosition.top);
        Console.SetCursorPosition(oldPosition.X, oldPosition.Y + 2);
        if (IsControlsVisible)
            DisplayControls();

        Console.SetCursorPosition(oldPosition.X, oldPosition.Y);
    }

    public static void DisplayControls()
    {
        Console.WriteLine("Controls:");
        Console.WriteLine(" - To open/hide controls - press `K`");
        Console.WriteLine(" - Moves in four directions are controlled by `W`, `S`, `A`, `D`");
        Console.WriteLine(" - To pick up an item - press `E`");
        Console.WriteLine(" - To drop an item - press `G`");
        Console.WriteLine(" - To equip/unequip an item - press `Q`. You can equip an item from inventory or from staying on the item, depending on your focus");
        Console.WriteLine(" - Use arrows to navigate through inventory, map items or eqquiped items");
        Console.WriteLine(" - To enter the inventory scope - press `I`, then use arrows to change the object");
        Console.WriteLine(" - To enter hands scope - press `H`, then use arrows to change the object");
        Console.WriteLine(" - To leave hands or inventory scope - press `Escape`");
    }

    public static void ChangeControlsVisibility() => IsControlsVisible = !IsControlsVisible;
}

//Console.WriteLine($"Current Focus (in {Object}): {output ?? "None"}");
//        Console.ForegroundColor = ConsoleColor.Red;
//        Console.Write("{ output ?? "None"}");
//        Console.ResetColor()
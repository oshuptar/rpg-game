using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.UnusableItems;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RPG_Game.HelperClasses;

public class ObjectDisplayer
{
    private static ObjectDisplayer? _objectDisplayerInstance;

    private ObjectDisplayer()
    {
        CurrentFocus = 0;
        FocusOn = FocusType.Room;
        IsControlsVisible = true;
    }

    public static ObjectDisplayer GetInstance()
    {
        if (_objectDisplayerInstance == null)
            ObjectDisplayer._objectDisplayerInstance = new ObjectDisplayer();
        return _objectDisplayerInstance;
    }

    public static int CurrentFocus { get; private set; } = 0;
    public static FocusType FocusOn { get; private set; } = FocusType.Room;
    private static (int left, int top) CursorPosition { get; set; }
    private static bool IsControlsVisible { get; set; } = true;

    public void ResetFocusIndex() => CurrentFocus = 0;
    public void SetInventoryFocus() => FocusOn = FocusType.Inventory;
    public void SetHandsFocus() => FocusOn = FocusType.Hands;
    public void ResetFocusType() => FocusOn = FocusType.Room;
    public  void DisplayControls(bool isControlsVisible = true) => Console.Write(ObjectRenderer.RenderControls(isControlsVisible));
    public StringBuilder DisplayInventory(Player player) => ObjectRenderer.RenderItemList(player.RetrieveInventory(), "Inventory");
    public StringBuilder DisplayEquipped(Player player) => ObjectRenderer.RenderItemList(player.RetrieveHands(), "Equipped");
    public StringBuilder DisplayTileItems(Room room, (int x, int y) position) => ObjectRenderer.RenderItemList(room.Items[position.x, position.y], "Items");
    public void ChangeControlsVisibility() => IsControlsVisible = !IsControlsVisible;
    public void FillLine() => Console.Write(ObjectRenderer.RenderEmptyLine());

    public void DisplayCurrent(List<IItem>? list, string Object)
    {
        string? output = null;
        if (list != null && list.Count != 0)
        {
            output = list[CurrentFocus].Name;
        }

        Console.Write($"Current Focus (in {Object}): ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{ output ?? "None"}");
        Console.ResetColor();
    }

    public void DisplayCurrentItem(Room room, Player player)
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

    public void ShiftCurrentFocus(Room room, Player player, Direction direction)
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
    public void ShiftFocus(List<IItem>? list, Direction direction)
    {
        if (list is null)
            return;

        switch (direction)
        {
            case Direction.Left:
                CurrentFocus = CurrentFocus - 1 >= 0 ? CurrentFocus - 1 : CurrentFocus;
                break;
            case Direction.Right:
                CurrentFocus = CurrentFocus + 1 <= list!.Count - 1 ? CurrentFocus + 1 : CurrentFocus;
                break;
        }
    }

    public void DisplayPlayerAttributes(Player player)
    {
        foreach (var key in player.RetrievePlayerStats().Attributes.Keys)
        { 
            Console.Write($"{key}: {player.RetrievePlayerStats().Attributes[key]}");
            FillLine();
            CursorPosition = (CursorPosition.left, CursorPosition.top + 1);
            Console.SetCursorPosition(CursorPosition.left, CursorPosition.top);
        }
    }

    public void WelcomeRoutine()
    {
        this.DisplayControls();
        Console.WriteLine(" - To Start the Game press any key");
        Console.WriteLine("Have fun!");
    }

    // Fix implementation
    public void DisplayRoutine(Room _room, Player player)
    {
        int noOfLists = 4;
        int verticalSpaceSize = Room._height / 20;
        int horizontalSpaceSize = 10;

        Console.SetCursorPosition(0, 0);
        Console.Write(ObjectRenderer.RenderGrid(_room));

        int horizontalPosition = Room._width + horizontalSpaceSize;
        int verticalPosition = verticalSpaceSize;

        (int X, int Y) oldPosition = Console.GetCursorPosition();

        Console.SetCursorPosition(horizontalPosition, verticalPosition);
        Console.Write(DisplayTileItems(_room, player.Position));
        FillLine();

        Console.SetCursorPosition(horizontalPosition, verticalPosition + 1);
        Console.Write(DisplayEquipped(player));
        FillLine();

        Console.SetCursorPosition(horizontalPosition, verticalPosition + 2);
        Console.Write(DisplayInventory(player));
        FillLine(); 

        Console.SetCursorPosition(horizontalPosition, verticalPosition + 3);
        DisplayCurrentItem(_room, player);
        FillLine();

        CursorPosition = (horizontalPosition, verticalPosition + noOfLists + verticalSpaceSize);
        Console.SetCursorPosition(CursorPosition.left, CursorPosition.top);
        DisplayPlayerAttributes(player);

        Console.SetCursorPosition(oldPosition.X, oldPosition.Y + verticalSpaceSize);
        
        DisplayControls(IsControlsVisible);

        Console.SetCursorPosition(oldPosition.X, oldPosition.Y);
    }
}
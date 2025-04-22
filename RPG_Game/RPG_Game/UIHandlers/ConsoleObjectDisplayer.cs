using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.LogMessages;
using RPG_Game.UnusableItems;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using RPG_Game.HelperClasses;
using System.Xml.Xsl;
using RPG_Game.Controller;

namespace RPG_Game.UIHandlers;

// state of the game in the console object displayer
public class ConsoleObjectDisplayer
{
    // Divide the console output

    private static ConsoleObjectDisplayer? _objectDisplayerInstance;
    private const int _verticalSpaceSize = Room._height / 10;
    private const int _horizontalSpaceSize = Room._width / 10;
    private Game _game;

    private int _noOfAttributes = 8; // hardcoded value; to change
    private int _noOfLists = 4; // hardcoded value; to change

    public int CurrentFocus { get; private set; }
    public FocusType FocusOn { get; private set; }
    private (int left, int top) CursorPosition { get; set; } = (0, 0);
    private bool IsControlsVisible { get; set; }

    private ConsoleObjectDisplayer()
    {
        CurrentFocus = 0;
        FocusOn = FocusType.Room;
        IsControlsVisible = true;
    }

    public void SetGame(Game game) => _game = game;

    public static ConsoleObjectDisplayer GetInstance()
    {
        if (_objectDisplayerInstance == null)
            ConsoleObjectDisplayer._objectDisplayerInstance = new ConsoleObjectDisplayer();
        return _objectDisplayerInstance;
    }

    public void AdjustFocusIndex()
    {
        CurrentFocus = CurrentFocus - 1 > 0 ? CurrentFocus - 1 : 0;
    }

    public void ResetFocusIndex()
    {
        if(FocusOn == FocusType.Room)
            CurrentFocus = 0;
    }
    public void SetInventoryFocus()
    {
        FocusOn = FocusType.Inventory;
        CurrentFocus = 0;
    }
    public void SetHandsFocus()
    {
        FocusOn = FocusType.Hands;
        CurrentFocus = 0;
    }
    public void ResetFocusType()
    {
        FocusOn = FocusType.Room;
        CurrentFocus = 0;
        
    }
    public void DisplayControls(bool isControlsVisible = true) => Console.Write(ObjectRenderer.GetInstance().RenderControls(isControlsVisible));
    public StringBuilder DisplayInventory(Player player) => ObjectRenderer.GetInstance().RenderItemList(player.RetrieveInventory(), "Inventory");
    public StringBuilder DisplayEquipped(Player player) => ObjectRenderer.GetInstance().RenderItemList(player.RetrieveHands(), "Equipped");
    public StringBuilder DisplayTileItems((int x, int y) position) => ObjectRenderer.GetInstance().RenderItemList(_game.GetRoom().RetrieveGrid()[position.x, position.y].Items, "Items");
    public void ChangeControlsVisibility() => IsControlsVisible = !IsControlsVisible;
    public void FillLine() => Console.Write(ObjectRenderer.GetInstance().RenderEmptyLine());

    public void DisplayCurrent(List<IItem>? list, string Object)
    {
        string? output = null;
        if (list != null && list.Count != 0)
        {
            output = list[CurrentFocus].Name;
        }

        Console.Write($"Current Focus (in {Object}): ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{output ?? "None"}");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void DisplayCurrentItem()
    {

        switch (FocusOn)
        {
            case FocusType.Inventory:
                DisplayCurrent(_game.GetPlayer().RetrieveInventory(), "Inventory");
                break;
            case FocusType.Hands:
                DisplayCurrent(_game.GetPlayer().RetrieveHands(), "Hands");
                break;
            case FocusType.Room:
                DisplayCurrent(_game.GetRoom().RetrieveGrid()[_game.GetPlayer().Position.x, _game.GetPlayer().Position.y].Items, "Room");
                break;
        }
    }

    public void ShiftCurrentFocus(Direction direction)
    {
        switch (FocusOn)
        {
            case FocusType.Inventory:
                ShiftFocus(_game.GetPlayer().RetrieveInventory(), direction);
                break;
            case FocusType.Hands:
                ShiftFocus(_game.GetPlayer().RetrieveHands(), direction);
                break;
            case FocusType.Room:
                ShiftFocus(_game.GetRoom().RetrieveGrid()[_game.GetPlayer().Position.x, _game.GetPlayer().Position.y].Items, direction);
                break;
        }
    }
    public void ShiftFocus(List<IItem>? list, Direction direction)
    {
        if (list is null)
            return;

        // Fix this enum
        switch (direction)
        {
            case Direction.West:
                CurrentFocus = CurrentFocus - 1 >= 0 ? CurrentFocus - 1 : CurrentFocus;
                break;
            case Direction.East:
                CurrentFocus = CurrentFocus + 1 <= list!.Count - 1 ? CurrentFocus + 1 : CurrentFocus;
                break;
        }
    }

    public void DisplayPlayerAttributes()
    {
        foreach (var key in _game.GetPlayer().RetrieveEntityStats().Attributes.Keys)
        {
            Console.Write($"{key}: {_game.GetPlayer().RetrieveEntityStats().Attributes[key]}");
            FillLine();
            CursorPosition = (CursorPosition.left, CursorPosition.top + 1);
            Console.SetCursorPosition(CursorPosition.left, CursorPosition.top);
        }
        Console.Write($"Attack Strategy: {_game.AttackType.ToString()}");
        FillLine();
    }

    public void WelcomeRoutine()
    {
        this.DisplayControls();
        Console.WriteLine(" - To Start the Game press any key");
        Console.WriteLine("Have fun!");
    }

    private void LogMessage(string message)
    {
        Console.SetCursorPosition(Room._width + _horizontalSpaceSize, _noOfLists + _noOfAttributes + 3 * _verticalSpaceSize);
        Console.Write($"{message}");
        FillLine();
    }
    public void LogMessage(OnPlayerDeathMessage messageInfo)
    {
        this.ClearConsole();
        Console.SetCursorPosition(0, 0);
        Console.Write($"{messageInfo.Player.Name} was killed. End of Game!");
    }

    private void LogWarning(string message)
    {
        Console.SetCursorPosition(Room._width + _horizontalSpaceSize, _noOfLists + _noOfAttributes + 4 * _verticalSpaceSize);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"\"{message}\"");
        Console.ForegroundColor = ConsoleColor.White;
        FillLine();
    }
    public void LogMessage(OnEnemyDetectionMessage messageInfo)
    {
        if (messageInfo.enemy != null)
            LogWarning($"Enemy Warning: {messageInfo.enemy} " +
                $"{messageInfo.enemy.RetrieveEntityStats().Attributes[PlayerAttributes.Health].GetCurrentValue()}HP" +
                $" at x:{messageInfo.enemy.Position.x}, y:{messageInfo.enemy.Position.y}");
    }

    public void LogMessage(OnEmptyDirectory messageInfo)
    {
        LogMessage($"{messageInfo.Name} emptied his inventory");
    }
    public void LogMessage(OnMoveMessage messageInfo)
    {
        LogMessage($"{messageInfo.Name} moved to the {messageInfo.direction}");
    }

    public void LogMessage(OnItemUnequipMessage messageInfo)
    {
        LogMessage($"{messageInfo.Name} unequipped {messageInfo.Item.Name}");
    }

    public void LogMessage(OnItemEquipMessage messageInfo)
    {
        LogMessage($"{messageInfo.Name} equipped {messageInfo.Item.Name} {messageInfo.Item.Description}");
    }

    public void LogMessage(OnItemDropMessage messageInfo)
    {
        LogMessage($"{messageInfo.Name} dropped {messageInfo.Item.Name}");
    }

    public void LogMessage(OnItemPickUpMessage messageInfo)
    {
        LogMessage($"{messageInfo.Name} picked up {messageInfo.Item.Name} {messageInfo.Item.Description}");
    }

    public void LogMessage(OnRequestNotSupportedMessage messageInfo)
    {
        LogMessage($"{messageInfo.Description}");
    }

    public void LogMessage(OnEnemyDeathMessage messageInfo)
    {
        LogMessage($"{messageInfo.enemy.ToString()} was killed");
    }

    public void LogMessage(OnAttackMessage messageInfo)
    {
        LogMessage($"{messageInfo.source} attacked {messageInfo.target}: -{messageInfo.Damage}HP");
    }
    public void ClearConsole() => Console.Clear();

    public void ClearLogMessage()
    {
        LogMessage(" ");
    }

    // We are overriding previous contents on Enemy type cells
    public void DisplayEnemies()
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        for (int i = 0; i < Room._width; i++)
        {
            for (int j = 0; j < Room._height; j++)
            {
                if ((_game.GetRoom().RetrieveGrid()[i, j].CellType & CellType.Enemy) != 0)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write(_game.GetRoom().RetrieveGrid()[i, j].PrintCell());
                }
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
    }

    // Fix implementation
    public void DisplayRoutine()
    {
        Console.SetCursorPosition(0, 0);
        Console.Write(ObjectRenderer.GetInstance().RenderGrid(_game.GetRoom()));

        (int X, int Y) oldPosition = Console.GetCursorPosition();

        DisplayEnemies();

        int horizontalPosition = Room._width + _horizontalSpaceSize;
        int verticalPosition = _verticalSpaceSize;

        Console.SetCursorPosition(horizontalPosition, verticalPosition);
        Console.Write(DisplayTileItems(_game.GetPlayer().Position));
        FillLine();

        Console.SetCursorPosition(horizontalPosition, verticalPosition + 1);
        Console.Write(DisplayEquipped(_game.GetPlayer()));
        FillLine();

        Console.SetCursorPosition(horizontalPosition, verticalPosition + 2);
        Console.Write(DisplayInventory(_game.GetPlayer()));
        FillLine();

        Console.SetCursorPosition(horizontalPosition, verticalPosition + 3);
        DisplayCurrentItem();
        FillLine();

        CursorPosition = (horizontalPosition, verticalPosition + _noOfLists + _verticalSpaceSize - 1);
        Console.SetCursorPosition(CursorPosition.left, CursorPosition.top);
        DisplayPlayerAttributes();

        CursorPosition = (horizontalPosition, Console.GetCursorPosition().Top + _verticalSpaceSize);

        Console.SetCursorPosition(oldPosition.X, oldPosition.Y + _verticalSpaceSize);

        DisplayControls(IsControlsVisible);

        Console.SetCursorPosition(oldPosition.X, oldPosition.Y);
    }
}
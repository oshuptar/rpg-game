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
using RPG_Game.Entities;
using RPG_Game.Model;

namespace RPG_Game.View;

public class ClientConsoleView : ClientView
{
    private static ClientConsoleView? ConsoleViewInstance;
    private const int _verticalSpaceSize = MapSettings.Height / 10;
    private const int _horizontalSpaceSize = MapSettings.Width / 10;
    private int _noOfAttributes = 8; // hardcoded value; to change
    private int _noOfLists = 4; // hardcoded value; to change
    private (int left, int top) CursorPosition { get; set; } = (0, 0);
    private bool IsControlsVisible { get; set; }
    private ClientConsoleView() : base(false)
    {
        IsControlsVisible = true;
    }
    public static ClientConsoleView GetInstance()
    {
        if (ConsoleViewInstance == null)
            ClientConsoleView.ConsoleViewInstance = new ClientConsoleView();
        return ConsoleViewInstance;
    }
    public void AdjustFocusIndex()
    {
        CurrentFocus = CurrentFocus - 1 > 0 ? CurrentFocus - 1 : 0;
    }
    public void ResetFocusIndex()
    {
        if(FocusOn == FocusType.Room)
            CurrentFocus = 0;
        //DisplayCurrentItem();
    }
    public override void SetInventoryFocus()
    {
        FocusOn = FocusType.Inventory;
        CurrentFocus = 0;
        DisplayRoutine();
    }
    public override void SetHandsFocus()
    {
        // FIx to only update needed parts
        FocusOn = FocusType.Hands;
        CurrentFocus = 0;
        DisplayRoutine();
    }
    public override void ResetFocusType()
    {
        FocusOn = FocusType.Room;
        CurrentFocus = 0;
        DisplayRoutine();
    }
    public void DisplayControls(bool isControlsVisible = true) => Console.Write(ObjectRenderer.GetInstance().RenderControls(isControlsVisible));
    public StringBuilder DisplayInventory(Player player) => ObjectRenderer.GetInstance().RenderItemList(player.GetInventory().GetInventoryState().Inventory, "Inventory");
    public StringBuilder DisplayEquipped(Player player) => ObjectRenderer.GetInstance().RenderItemList(player.GetHands().GetHandState().Hands, "Equipped");
    public StringBuilder DisplayTileItems(Position position) => ObjectRenderer.GetInstance().RenderItemList(GameState.GetItems(position), "Items");
    public override void HideControls() => IsControlsVisible = !IsControlsVisible;
    public void FillLine() => Console.Write(ObjectRenderer.GetInstance().RenderEmptyLine());
    public void DisplayCurrent(List<Item>? list, string Object)
    {
        string? output = null;
        if (list != null && list.Count != 0)
        {
            output = list[Math.Min(CurrentFocus,list.Count - 1)].Name;
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
                DisplayCurrent(GameState.GetPlayer().GetInventory().GetInventoryState().Inventory, "Inventory");
                break;
            case FocusType.Hands:
                DisplayCurrent(GameState.GetPlayer().GetHands().GetHandState().Hands, "Hands");
                break;
            case FocusType.Room:
                DisplayCurrent(GameState.GetItems(GameState.GetPlayer().Position), "Room");
                break;
        }
    }
    public override void ShiftCurrentFocus(Direction direction)
    {
        switch (FocusOn)
        {
            case FocusType.Inventory:
                ShiftFocus(GameState.GetPlayer().GetInventory().GetInventoryState().Inventory, direction);
                break;
            case FocusType.Hands:
                ShiftFocus(GameState.GetPlayer().GetHands().GetHandState().Hands, direction);
                break;
            case FocusType.Room:
                ShiftFocus(GameState.GetItems(GameState.GetPlayer().Position), direction);
                break;
        }
        DisplayRoutine();
    }
    public void ShiftFocus(List<Item>? list, Direction direction)
    {
        if (list is null)
            return;

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
        foreach (var key in GameState.GetPlayer().GetEntityStats().Attributes.Keys)
        {
            Console.Write($"{key}: {GameState.GetPlayer().GetEntityStats().Attributes[key]}");
            FillLine();
            CursorPosition = (CursorPosition.left, CursorPosition.top + 1);
            Console.SetCursorPosition(CursorPosition.left, CursorPosition.top);
        }
        Console.Write($"Attack Strategy: {GameState.GetPlayer().AttackType.ToString()}");
        FillLine();
    }

    public override void WelcomeRoutine()
    {
        this.DisplayControls();
        Console.WriteLine(" - To Start the Game press any key");
        Console.WriteLine("Have fun!");
    }
    public override void ClearLogMessage()
    {
        LogMessage(" ");
    }
    //public override void EndRoutine(bool flag)
    //{
    //    throw new NotImplementedException();
    //}
    public override void DisplayRoutine()
    {
        Console.SetCursorPosition(0, 0);
        Console.Write(ObjectRenderer.GetInstance().RenderMap(GameState));

        (int X, int Y) oldPosition = Console.GetCursorPosition();
        //DisplayEnemies();

        int horizontalPosition = MapSettings.Width + _horizontalSpaceSize;
        int verticalPosition = _verticalSpaceSize;

        Console.SetCursorPosition(horizontalPosition, verticalPosition);
        Console.Write(DisplayTileItems(GameState.GetPlayer().Position));
        FillLine();

        Console.SetCursorPosition(horizontalPosition, verticalPosition + 1);
        Console.Write(DisplayEquipped(GameState.GetPlayer()));
        FillLine();

        Console.SetCursorPosition(horizontalPosition, verticalPosition + 2);
        Console.Write(DisplayInventory(GameState.GetPlayer()));
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
    public void LogMessage(string message)
    {
        Console.SetCursorPosition(MapSettings.Width + _horizontalSpaceSize, _noOfLists + _noOfAttributes + 3 * _verticalSpaceSize);
        Console.Write($"{message}");
        FillLine();
    }

    // We are overriding previous contents on Enemy type cells
    //public void DisplayEnemies()
    //{
    //    Console.ForegroundColor = ConsoleColor.DarkRed;
    //    for (int i = 0; i < MapSettings.Width; i++)
    //    {
    //        for (int j = 0; j < MapSettings.Height; j++)
    //        {
    //            if ((Game.GetRoom().GetRoomState().GetGrid()[i, j].CellType & CellType.Enemy) != 0)
    //            {
    //                Console.SetCursorPosition(i, j);
    //                Console.Write(Game.GetRoom().GetRoomState().GetGrid()[i, j].PrintCell());
    //            }
    //        }
    //    }
    //    Console.ForegroundColor = ConsoleColor.White;
    //}

    // Fix implementation
    //public void LogMessage(OnPlayerDeathMessage messageInfo)
    //{
    //    this.ClearConsole();
    //    Console.SetCursorPosition(0, 0);
    //    Console.Write($"{messageInfo.Player.Name} was killed. End of Game!");
    //}
    //private void LogWarning(string message)
    //{
    //    Console.SetCursorPosition(MapSettings.Width + _horizontalSpaceSize, _noOfLists + _noOfAttributes + 4 * _verticalSpaceSize);
    //    Console.ForegroundColor = ConsoleColor.Red;
    //    Console.Write($"\"{message}\"");
    //    Console.ForegroundColor = ConsoleColor.White;
    //    FillLine();
    //}
    //public void LogMessage(OnEnemyDetectionMessage messageInfo)
    //{
    //    if (messageInfo.enemy != null)
    //        LogWarning($"Enemy Warning: {messageInfo.enemy} " +
    //            $"{messageInfo.enemy.GetEntityStats().Attributes[PlayerAttributes.Health].GetCurrentValue()}HP" +
    //            $" at x:{messageInfo.enemy.Position.X}, y:{messageInfo.enemy.Position.Y}");
    //}

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
    //public void ClearConsole() => Console.Clear();
}

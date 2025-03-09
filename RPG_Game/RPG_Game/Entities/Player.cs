using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entiities;

public class Player : ICanMove, ICanReceiveDamage
{
    // Add separate classes for inventory responsibiillitiies and hands responisiblities
    public (int x, int y) Position { get; private set; }
    public List<IItem> inventory { get; } = new List<IItem>();
    public List<IItem> hands { get; } = new List<IItem>(); //technicaly we cn equip any item to use it later on like elixir

    public const int MaxCapacity = 2; // MaxCapacity of Hands
    public int Capacity { get; private set; } = 0;
    public PlayerStats PlayerStats { get; private set; }
    public Player()
    {
        Position = (1, 1);
        PlayerStats = new PlayerStats();
    }

    public (int, int) GetNewPosition(Direction direction)
    {
        (int x, int y) TempPos = Position;
        switch (direction)
        {
            case Direction.Left:
                TempPos.x -= 1;
                break;
            case Direction.Right:
                TempPos.x += 1;
                break;
            case Direction.Up:
                TempPos.y -= 1;
                break;
            case Direction.Down:
                TempPos.y += 1;
                break;
        }
        return TempPos;
    }

    public (int,int)? IsMovable(Direction direction, Room room) // whether we can move in the following direction
    {
        (int x, int y) TempPos = GetNewPosition(direction);

        if (!room.IsPosAvailable(TempPos.x, TempPos.y))
            return null;

        return TempPos;
    }

    public bool Move(Direction direction, Room room)
    {
        (int, int)? TempPos;
        if((TempPos = IsMovable(direction, room)) != null)
        {
            room.RemoveObject(CellType.Player, Position);
            room.AddObject(CellType.Player, ((int, int)) TempPos);
            Position = ((int, int))TempPos;
            return true;
        }
        return false;
    }

    public void ReceiveDamage(int damage) { }// To implement

    // Must change player's attrbutes when needed
    public void PickUp(IItem? item)
    {
        if (item == null)
            return;
        inventory.Add(item);
        item.ApplyChanges(this);
    }

    public bool Equip(IItem? item, bool isInInventory = true)
    {
        if (item == null || item.Capacity + Capacity > MaxCapacity)
            return false;

        hands.Add(item);
        Capacity += item.Capacity;

        if(!isInInventory)
            item.ApplyChanges(this);

        return true;
    }

    public void UnEquip(int index)
    {
        IItem? item = hands.ElementAt(index);
        hands.Remove(item);
        inventory.Add(item);
        Capacity -= item.Capacity;
    }

    public IItem? Retrieve(int index, List<IItem> list)
    {
        if (list.Count == 0)
            return null;
        IItem item = list.ElementAt(index);
        return item;
    }

    public IItem? Remove(int index, List<IItem> list)
    {
        IItem? item = Retrieve(index, list);
        if(item != null)
            list.RemoveAt(index);
        return item;
    }

    public IItem? Drop(Room room, int index, List<IItem> list)
    {
        IItem? item = Remove(index, list);
        if (item != null)
        {
            room.AddItem(item, (Position.x, Position.y));
            item.RevertChanges(this);
        }
        return item;
    }

    public IItem? DropFromHands(Room room, int index)
    {
        IItem? item = Drop(room, index, hands);

        if(item != null)
            Capacity -= item.Capacity;
        return item;

    }
    public IItem? DropFromInventory(Room room, int index)
    {
        return Drop(room, index, inventory);
    }
}

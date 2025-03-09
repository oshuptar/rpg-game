using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public class Player : ICanMove, ICanReceiveDamage
{
    // Add separate classes for inventory responsibiillitiies and hands responisiblities
    public (int x, int y) Position { get; private set; }
    public List<IItem> inventory { get; } = new List<IItem>();
    public List<IItem> hands { get; } = new List<IItem>(); //technicaly we cn equip any item to use it later on like elixir

    public const int MaxCapacity = 2; // MaxCapacity of Hands
    public int Capacity { get; private set; } = 0;
    public int TotalMoneyValue { get; private set; } = 50; // 50 as a default value
    public int CollectedCoins { get; private set; } = 0;
    public int CollectedGold { get; private set; } = 0;
    public Dictionary<string, int> Attributes { get; private set; } = new Dictionary<string, int>();
    public Player()
    {
        Position = (1, 1);

        Attributes.Add("Health", 100);
        Attributes.Add("Strength", 0);
        Attributes.Add("Luck", 0);
        Attributes.Add("Aggression", 0);
        Attributes.Add("Dexterity", 2);
    }

    public (int, int) GetNewPosition(Direction direction)
    {
        (int x, int y) TempPos = this.Position;
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
            this.Position = ((int, int))TempPos;
            return true;
        }
        return false;
    }

    public void ReceiveDamage(int damage) // To implement
    {
        Attributes["Health"] = (Attributes["Health"] - damage < 0) ? 0 : Attributes["Health"] - damage;
    }

    // Must change player's attrbutes when needed
    public void PickUp(IItem? item)
    {
        if (item == null)
            return;
        inventory.Add(item);
    }

    public bool Equip(IItem? item)
    {
        if (item == null || item.Capacity + this.Capacity > MaxCapacity)
            return false;
        hands.Add(item);
        this.Capacity += item.Capacity;
        return true;
    }

    public void UnEquip(int index)
    {
        IItem? weapon = hands.ElementAt(index);
        hands.Remove(weapon);
        inventory.Add(weapon);
        this.Capacity -= weapon.Capacity;
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
        if(item != null)
            room.AddItem(item, (this.Position.x, this.Position.y));
        //item.Drop(this);
        return item;
    }

    public IItem? DropFromHands(Room room, int index)
    {
        IItem? item = Drop(room, index, this.hands);

        if(item != null)
            this.Capacity -= item.Capacity;
        return item;

    }
    public IItem? DropFromInventory(Room room, int index)
    {
        return Drop(room, index, this.inventory);
    }

    public void AddCoin(ICurrency coin)
    {
        CollectedCoins++;
        TotalMoneyValue += coin.Value;
    }
    public void AddGold(ICurrency gold)
    {
        CollectedGold++;
        TotalMoneyValue += gold.Value;
    }
}

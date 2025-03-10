using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entiities;

public class Player : ICanMove, ICanReceiveDamage
{
    public (int x, int y) Position { get; private set; }
    public PlayerStats PlayerStats { get; private set; } = new PlayerStats();

    private Hands Hands = new Hands();

    private Inventory Inventory = new Inventory();
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
        if ((TempPos = IsMovable(direction, room)) != null)
        {
            room.RemoveObject(CellType.Player, Position);
            room.AddObject(CellType.Player, ((int, int))TempPos);
            Position = ((int, int))TempPos;
            return true;
        }
        return false;
    }
    public void ReceiveDamage(int damage) { }// To implement
    // Must change player's attrbutes when needed
    public void PickUp(IItem? item) => Inventory.PickUp(item, this);
    public bool Equip(IItem? item, bool isInInventory = true) => Hands.Equip(item, this, isInInventory);
    public void UnEquip(int index)
    {
        IItem? item = Hands.UnEquip(index);
        if(item != null)
            Inventory.inventory.Add(item);
    }
    public IItem? Retrieve(int index, bool fromInventory)
    {
        if (fromInventory)
            return Inventory.Retrieve(index, Inventory.inventory);
        return Hands.Retrieve(index, Hands.hands);
    }
    public IItem? Remove(int index, bool fromInventory)
    {
        if (fromInventory)
            return Inventory.Remove(index, Inventory.inventory);
        return Hands.Remove(index, Hands.hands);
    }
    public IItem? Drop(Room room, int index, bool fromInventory)
    {
        if (fromInventory)
            return Inventory.DropFromInventory(room, index, this);
        return Hands.DropFromHands(room, index, this);
    }
    public List<IItem> RetrieveHands() => this.Hands.hands;
    public List<IItem> RetrieveInventory() => this.Inventory.inventory;

}
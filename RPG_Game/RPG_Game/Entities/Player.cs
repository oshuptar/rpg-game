using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.HelperClasses;
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

    public string Name = "Raikuro Takeda";

    private PlayerStats PlayerStats  = new PlayerStats();

    private Hands Hands = new Hands();

    private Inventory Inventory = new Inventory();
    public Player()
    {
        Position = (1, 1);
    }

    public (int, int) GetNewPosition(Direction direction)
    {
        (int x, int y) TempPos = Position;
        switch (direction)
        {
            case Direction.West:
                TempPos.x -= 1;
                break;
            case Direction.East:
                TempPos.x += 1;
                break;
            case Direction.North:
                TempPos.y -= 1;
                break;
            case Direction.South:
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

    //By default the order in LINQ is ascending

    public bool Move(Direction direction, Room room)
    {
        (int, int)? TempPos;
        if ((TempPos = IsMovable(direction, room)) != null)
        {
            room.RemoveObject(CellType.Player, Position);
            room.AddObject(CellType.Player, ((int, int))TempPos);
            Position = ((int, int))TempPos;
            ObjectDisplayer.GetInstance().LogMessage($"{Name} moved to the {direction}");
            //Extract the enemy which is closest to the player given the cartesian product distance
            IEnemy? enemy = room.Enemies.MinBy((enemy) => Math.Sqrt(Math.Pow(this.Position.x - enemy.Position.x, 2) + Math.Pow(this.Position.y -  enemy.Position.y, 2)));
            ObjectDisplayer.GetInstance().LogWarning($"Enemy Warning: {enemy} at x:{enemy?.Position.x}, y:{enemy?.Position.y}");
            return true;
        }
        return false;
    }

    public void PlacePlayer(Room room) => room.RetrieveGrid()[Position.x, Position.y].CellType |= CellType.Player;
    public void ReceiveDamage(int damage) { }// To implement
    public void PickUp(IItem? item)
    {
        Inventory.PickUp(item, this); //changes player's attrbutes when picked up
        if(item != null)
            ObjectDisplayer.GetInstance().LogMessage($"{Name} picked up {item.Name} {item.Description}");
    }
    public bool Equip(IItem? item, bool isInInventory = true)
    {
        bool isEquipped =  Hands.Equip(item, this, isInInventory);
        if(isEquipped)
            ObjectDisplayer.GetInstance().LogMessage($"{Name} equipped {item!.Name} {item.Description}");
        return isEquipped;
    }
    public void UnEquip(int index)
    {
        IItem? item = Hands.UnEquip(index);
        if (item != null)
        {
            Inventory.inventory.Add(item);
            ObjectDisplayer.GetInstance().LogMessage($"{Name} unequipped {item?.Name}");
        }
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
        IItem? item;
        if (fromInventory)
            item = Inventory.DropFromInventory(room, index, this);
        item = Hands.DropFromHands(room, index, this);

        if(item != null)
            ObjectDisplayer.GetInstance().LogMessage($"{Name} dropped {item.Name}");

        return item;
    }
    public List<IItem> RetrieveHands() => this.Hands.hands;
    public List<IItem> RetrieveInventory() => this.Inventory.inventory;
    public PlayerStats RetrievePlayerStats() => this.PlayerStats;

}
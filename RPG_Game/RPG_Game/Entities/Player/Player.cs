using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.HelperClasses;
using RPG_Game.Interfaces;
using RPG_Game.LogMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.UIHandlers;

namespace RPG_Game.Entiities;


public class Player : IEntity
{
    public event EventHandler? EntityMoved;
    public (int x, int y) Position { get; set; }

    public string Name = "Daymio Harutosi";

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
    
    protected virtual void OnMoveEvent()
    {
        EntityMoved?.Invoke(this, EventArgs.Empty);
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
            //Extract the enemy which is closest to the player given the cartesian product distance
            IEnemy? enemy = room.Enemies.MinBy((enemy) => Math.Sqrt(Math.Pow(this.Position.x - enemy.Position.x, 2) + Math.Pow(this.Position.y -  enemy.Position.y, 2)));
            OnMoveEvent();

            ConsoleObjectDisplayer.GetInstance().LogMessage(new OnEnemyDetectionMessage(enemy));
            ConsoleObjectDisplayer.GetInstance().LogMessage(new OnMoveMessage(direction, this.Name));
            return true;
        }
        return false;
    }

    public void PlacePlayer(Room room) => room.RetrieveGrid()[Position.x, Position.y].CellType |= CellType.Player;
    public void ReceiveDamage(int damage) { }// To implement
    //We can implement PickUp as Visitor
    public void PickUp(IItem? item)
    {
        Inventory.PickUp(item, this); //changes player's attrbutes when picked up
        if(item != null)
            ConsoleObjectDisplayer.GetInstance().LogMessage(new OnItemPickUpMessage(item, this.Name));
    }
    public bool Equip(IItem? item, bool isInInventory = true)
    {
        bool isEquipped = Hands.Equip(item, this, isInInventory);
        if(isEquipped)
            ConsoleObjectDisplayer.GetInstance().LogMessage(new OnItemEquipMessage(item!, this.Name));
        return isEquipped;
    }
    public void UnEquip(int index)
    {
        IItem? item = Hands.UnEquip(index);
        if (item != null)
        {
            Inventory.inventory.Add(item);
            ConsoleObjectDisplayer.GetInstance().LogMessage(new OnItemUnequipMessage(item, this.Name));
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
        else 
            item = Hands.DropFromHands(room, index, this);

        if(item != null)
            ConsoleObjectDisplayer.GetInstance().LogMessage(new OnItemDropMessage(item, this.Name));

        return item;
    }

    public void EmptyInventory(Room room)
    {
        Inventory.EmptyInventory(room, this);
        ConsoleObjectDisplayer.GetInstance().LogMessage(new OnEmptyDirectory(this.Name));
    }
    public List<IItem> RetrieveHands() => this.Hands.hands;
    public List<IItem> RetrieveInventory() => this.Inventory.inventory;
    public EntityStats RetrieveEntityStats() => this.PlayerStats;

    public object Copy()
    {
        return new Player();
    }
}
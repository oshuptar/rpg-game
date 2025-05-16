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
    public event EventHandler? OwnDeath;
    public Position Position { get; set; }
    public string Name = "Daymio Harutosi";
    private PlayerStats PlayerStats  = new PlayerStats();
    private Hands Hands = new Hands();
    private Inventory Inventory = new Inventory();
    public Player()
    {
        this.PlayerStats.Died += OwnDeathHandler;
        Position = new Position(1, 1);
    }
    public Position GetNewPosition(Direction direction)
    {
        Position tempPos = Position;
        switch (direction)
        {
            case Direction.West:
                tempPos.X -= 1;
                break;
            case Direction.East:
                tempPos.X += 1;
                break;
            case Direction.North:
                tempPos.Y -= 1;
                break;
            case Direction.South:
                tempPos.Y += 1;
                break;
        }
        return tempPos;
    }
    protected void OwnDeathHandler(object sender, EventArgs e)
    {
        this.OwnDeath?.Invoke(this, e);
    }
    protected virtual void OnMoveEvent()
    {
        EntityMoved?.Invoke(this, EventArgs.Empty);
    }
    public Position? IsMovable(Direction direction, Room room) // whether we can move in the following direction
    {
        Position tempPos = GetNewPosition(direction);

        if (!room.IsPosAvailable(tempPos))
            return null;

        return tempPos;
    }
    //By default the order in LINQ is ascending
    public bool Move(Direction direction, Room room)
    {
        Position? tempPos;
        if ((tempPos = IsMovable(direction, room)) != null)
        {
            room.RemoveObject(CellType.Player, Position);
            room.AddObject(CellType.Player, tempPos);

            Position = tempPos;
            IEnemy? enemy = room.GetRoomState().Enemies.MinBy((enemy) => Math.Sqrt(Math.Pow(Position.X - enemy.Position.X, 2) + Math.Pow(Position.Y -  enemy.Position.Y, 2)));
            OnMoveEvent();

            ClientConsoleView.GetInstance().LogMessage(new OnEnemyDetectionMessage(enemy));
            ClientConsoleView.GetInstance().LogMessage(new OnMoveMessage(direction, this.Name));
            return true;
        }
        return false;
    }
    public void PlacePlayer(Room room) => room.GetRoomState().GetGrid()[Position.X, Position.Y].CellType |= CellType.Player;
    public void ReceiveDamage(int damage, IEntity? source) 
    { 
        PlayerStats.ModifyEntityAttribute(PlayerAttributes.Health, -damage);
    }
    public void PickUp(IItem? item)
    {
        Inventory.PickUp(item, this); //changes player's attrbutes when picked up
        if(item != null)
            ClientConsoleView.GetInstance().LogMessage(new OnItemPickUpMessage(item, this.Name));
    }
    public bool Equip(IItem? item, bool isInInventory = true)
    {
        bool isEquipped = Hands.Equip(item, this, isInInventory);
        if(isEquipped)
            ClientConsoleView.GetInstance().LogMessage(new OnItemEquipMessage(item!, this.Name));
        return isEquipped;
    }
    public void UnEquip(int index)
    {
        IItem? item = Hands.UnEquip(index);
        if (item != null)
        {
            Inventory.AddItem(item);
            ClientConsoleView.GetInstance().LogMessage(new OnItemUnequipMessage(item, this.Name));
        }
    }
    public IItem? Retrieve(int index, bool fromInventory)
    {
        if (fromInventory)
            return Inventory.Retrieve(index, Inventory.GetInventoryState().Inventory);
        return Hands.Retrieve(index, Hands.GetHandState().Hands);
    }
    public IItem? Remove(int index, bool fromInventory)
    {
        if (fromInventory)
            return Inventory.Remove(index, Inventory.GetInventoryState().Inventory);
        return Hands.Remove(index, Hands.GetHandState().Hands);
    }
    public IItem? Drop(Room room, int index, bool fromInventory)
    {
        IItem? item;
        if (fromInventory)
            item = Inventory.DropFromInventory(room, index, this);
        else 
            item = Hands.DropFromHands(room, index, this);

        if(item != null)
            ClientConsoleView.GetInstance().LogMessage(new OnItemDropMessage(item, this.Name));

        return item;
    }
    public void EmptyInventory(Room room)
    {
        Inventory.EmptyInventory(room, this);
        ClientConsoleView.GetInstance().LogMessage(new OnEmptyDirectory(this.Name));
    }
    public Hands GetHands() => this.Hands;
    public Inventory GetInventory() => this.Inventory;
    public EntityStats GetEntityStats() => this.PlayerStats;
    public object Copy()
    {
        return new Player();
    }
    public override string ToString()
    {
        return this.Name;
    }
}
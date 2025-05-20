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
using RPG_Game.Model;
using RPG_Game.Model.Entities;
using RPG_Game.Controller;
using System.Text.Json.Serialization;

namespace RPG_Game.Model;

public class Player : Entity
{
    [JsonInclude]
    public AttackType AttackType { get; set; } = AttackType.NormalAttack;
    [JsonInclude]
    public AttackStrategy AttackStrategy { get; set; } = new NormalAttackStrategy();
    [JsonInclude]
    public int PlayerId { get; set; }
    [JsonInclude]
    public string Name { get; private set; }
    [JsonInclude]
    public Hands Hands { get; private set; } = new Hands();
    [JsonInclude]
    public Inventory Inventory { get; private set; } = new Inventory();
    [JsonInclude]
    public PlayerStats PlayerStats { get; private set; } = new PlayerStats();
    public Player() : base()
    {}
    public Player(int playerId) : base()
    {
        this.PlayerStats.Died += OnDeath;

        Position = new Position(1, 1);
        PlayerId = playerId;
        Name += playerId.ToString();
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
    public Position? IsMovable(Direction direction, AuthorityGameState room) // whether we can move in the following direction
    {
        Position tempPos = GetNewPosition(direction);

        if (!room.IsPosAvailable(tempPos))
            return null;

        return tempPos;
    }
    public override bool Move(Direction direction, AuthorityGameState room)
    {
        Position? tempPos;
        if ((tempPos = IsMovable(direction, room)) != null)
        {
            room.RemovePlayer(this);
            room.AddPlayer(this, tempPos.Value);
            //OnMove();

            Entity? entity = room.GetClosestEntity(this);

            //ClientConsoleView.GetInstance().LogMessage(new OnEnemyDetectionMessage(enemy));
            //ClientConsoleView.GetInstance().LogMessage(new OnMoveMessage(direction, this.Name));
            return true;
        }
        return false;
    }
    //public void PlacePlayer(Room room)
    //{
    //    room.GetRoomState().GetGrid()[Position.X, Position.Y].CellType |= CellType.Player;
    //    if(room.GetRoomState().GetGrid()[Position.X, Position.Y].Entity == null)
    //        room.GetRoomState().GetGrid()[Position.X, Position.Y].Entity = this;
    //}
    public override void ReceiveDamage(int damage, Entity? source) 
    { 
        PlayerStats.ModifyEntityAttribute(PlayerAttributes.Health, -damage);
    }
    public void PickUp(Item? item)
    {
        Inventory.PickUp(item, this);
        //if(item != null)
        //    ClientConsoleView.GetInstance().LogMessage(new OnItemPickUpMessage(item, this.Name));
    }
    public bool Equip(Item? item, bool isInInventory = true)
    {
        return Hands.Equip(item, this, isInInventory);
        //if(isEquipped)
        //    ClientConsoleView.GetInstance().LogMessage(new OnItemEquipMessage(item!, this.Name));
        //return isEquipped;
    }
    public void UnEquip(int index)
    {
        Item? item = Hands.UnEquip(index);
        if (item != null)
            Inventory.AddItem(item);
            //ClientConsoleView.GetInstance().LogMessage(new OnItemUnequipMessage(item, this.Name));  
    }
    public Item? Retrieve(int index, bool fromInventory)
    {
        if (fromInventory)
            return Inventory.Retrieve(index, Inventory.GetInventoryState().Inventory);
        return Hands.Retrieve(index, Hands.GetHandState().Hands);
    }
    public Item? Remove(int index, bool fromInventory)
    {
        if (fromInventory)
            return Inventory.Remove(index, Inventory.GetInventoryState().Inventory);
        return Hands.Remove(index, Hands.GetHandState().Hands);
    }
    public Item? Drop(AuthorityGameState room, Item item, bool fromInventory)
    {
        Item? droppedItem = null;
        if(fromInventory)
        {
            int? index = Inventory.GetInventoryState().Inventory.IndexOf(item);
            if(index != null)
                droppedItem = Inventory.DropFromInventory(room, index.Value, this);
        }
        else
        {
            int? index = Hands.GetHandState().Hands.IndexOf(item);
            if (index != null)
                droppedItem = Inventory.DropFromInventory(room, index.Value, this);
        }
        return droppedItem;
    }
    public Item? Drop(AuthorityGameState room, int index, bool fromInventory)
    {
        Item? item;
        if (fromInventory)
            item = Inventory.DropFromInventory(room, index, this);
        else 
            item = Hands.DropFromHands(room, index, this);
        //if(item != null)
        //    ClientConsoleView.GetInstance().LogMessage(new OnItemDropMessage(item, this.Name));
        return item;
    }
    public void EmptyInventory(AuthorityGameState room)
    {
        Inventory.EmptyInventory(room, this);
        //ClientConsoleView.GetInstance().LogMessage(new OnEmptyDirectory(this.Name));
    }
    public Hands GetHands() => this.Hands;
    public Inventory GetInventory() => this.Inventory;
    public override EntityStats GetEntityStats() => this.PlayerStats;
    public override object Copy()
    {
        return new Player(PlayerId);
    }
    public override string ToString()
    {
        return this.Name;
    }
    public void SetAttackMode(AttackType attackType, AttackStrategy attackStrategy)
    {
        AttackType = attackType;
        AttackStrategy = attackStrategy;
    }
}

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
    public override bool Move(Direction direction, AuthorityGameState room)
    {
        Position? tempPos;
        if ((tempPos = IsMovable(direction, room)) != null)
        {
            room.RemovePlayer(this);
            room.AddPlayer(this, tempPos.Value);
            OnMove(direction);
            return true;
        }
        return false;
    }
    public override void ReceiveDamage(int damage, Entity? source) 
    { 
        PlayerStats.ModifyEntityAttribute(PlayerAttributes.Health, -damage);
    }
    public void PickUp(Item? item)
    {
        Inventory.PickUp(item, this);
    }
    public bool Equip(Item? item, bool isInInventory = true)
    {
        return Hands.Equip(item, this, isInInventory);
    }
    public void UnEquip(int index)
    {
        Item? item = Hands.UnEquip(index);
        if (item != null)
            Inventory.AddItem(item);
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
        return item;
    }
    public void EmptyInventory(AuthorityGameState room)
    {
        Inventory.EmptyInventory(room, this);
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
    public override void Attack(Entity target)
    {
        //throw new NotImplementedException();
    }
}

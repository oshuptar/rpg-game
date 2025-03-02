using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

public class Player
{
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Health { get; set; }
    public int Luck { get; set; }
    public int Aggresion { get; set; }

    private (int x, int y) Position;

    private List<IItem> inventory = new List<IItem>();

    private List<IItem> hands = new List<IItem>();

    public const int MaxCapacity = 2; // MaxCapacity of Hands
    public int Capacity { get; private set; } = 0;
    public int TotalMoneyValue { get; private set; } = 50; // 50 as a default value
    public int CollectedCoins { get; private set; } = 0;
    public int CollectedGold { get; private set; } = 0;
    public Player()
    {
        Strength = 0;
        Dexterity = 2; // Probably will affect the rendering rate of the map, this value will likely represent the cooldown value
        Health = 100;
        Luck = 0;
        Aggresion = 0;
        Position = (1, 1);
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
        Health = (Health - damage < 0) ? 0 : Health - damage;
    }

    public void PickUp(IItem item)
    {
        inventory.Add(item);
    }

    public void Equip(IWeapon weapon)
    {
        if (weapon.Capacity + this.Capacity > 2)
        {
            Console.WriteLine("This weapon cannot be equipped. Free some space");
            return;
        }
        hands.Add(weapon);
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

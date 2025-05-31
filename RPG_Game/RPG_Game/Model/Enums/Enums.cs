using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Enums;

public enum PlayerAttributes
{
    Health,
    Strength,
    Luck,
    Aggression,
    Dexterity,
    Coins,
    Gold,
    Money,
    Attack,
    Armor,
    Wisdom,
};

public enum AttackType
{
    NormalAttack,
    StealthAttack,
    MagicAttack,
};

[Flags] // This allows combining the enum values using bitwise operators
public enum CellType
{
    Empty = 0,
    Wall = 1,
    Player = 2,
    Item = 4,
    Enemy = 8,
};

public enum Direction
{
    West,
    East,
    North,
    South
};

public enum FocusType
{
    Room,
    Inventory,
    Hands,
};

public enum RequestType
{
    MoveUp,
    UseItem,
    MoveDown,
    MoveRight,
    MoveLeft,
    DropItem,
    PickUpItem,
    EquipItem, // This is responsible both for equipping and unequipping
    ScopeHands,
    ScopeInventory,
    EmptyInventory,
    ScopeRoom,
    HideControls,
    NextItem,
    PrevItem,
    OneWeaponAttack,
    TwoWeaponAttack,
    NormalAttack,
    StealthAttack,
    MagicAttack,
    EnemyAttack,
    Quit,
    Ignore,
    ServerStop,
    ClientJoined,
};
public static class DirectionTranslator
{
    public static (int, int) TranslateDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.East:
                return (1, 0);
            case Direction.West:
                return (-1, 0);
            case Direction.North:
                return (0, -1);
            case Direction.South:
                return (0, 1);
            default: return (0, 0);
        }
    }
}


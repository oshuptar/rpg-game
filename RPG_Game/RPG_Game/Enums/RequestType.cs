using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Enums;

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
}

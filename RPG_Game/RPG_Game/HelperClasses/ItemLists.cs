using RPG_Game.Currencies;
using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Interfaces;
using RPG_Game.Model.Entities;
using RPG_Game.Potions;
using RPG_Game.UnusableItems;
using RPG_Game.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.HelperClasses;

public class ItemLists
{
    public List<Item> ItemList = new List<Item>()
    {
        new Gold(),
        new Coin(),
        new Key(),
        new Note(),
        new Lore()
    };
    public List<Item> DecoratedItemList = new List<Item>()
    {
        new LuckItemDecorator(new Key()),
        new LuckItemDecorator(new Gold()),
        new LuckItemDecorator(new Coin()),
        new LuckItemDecorator(new LuckItemDecorator(new Lore())),
        new LuckItemDecorator(new LuckItemDecorator (new Coin())),
    };
    public List<Item> WeaponList = new List<Item>()
    {
        new Hammer(),
        new Sword(),
        new Dagger(),
        new Sword()
    };
    public List<Item> DecoratedWeaponList = new List<Item>()
    {
        new PowerWeaponDecorator(new Hammer()),
        new PowerWeaponDecorator(new PowerWeaponDecorator(new Hammer())),
        new Sword(),
        new Dagger(),
        new PowerWeaponDecorator(new Dagger()),
        new PowerWeaponDecorator(new Sword()),
        new PowerWeaponDecorator(new PowerWeaponDecorator(new Sword())),
    };
    public List<Entity> EnemyList = new List<Entity>()
    {
        new Goblin(),
        new Orc(),
    };
    public List<Potion> PotionList = new List<Potion>()
    {
        new HealPotion(),
        new AggressionPotion(),
        new DexterityPotion(),
    };
    private static ItemLists? _itemListInstance;
    private ItemLists() { }
    public static ItemLists GetInstance()
    {
        if(_itemListInstance == null)
            _itemListInstance = new ItemLists();
        return _itemListInstance;
    }
}

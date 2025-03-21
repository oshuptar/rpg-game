using RPG_Game.Currencies;
using RPG_Game.Entiities;
using RPG_Game.Interfaces;
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
    public List<IItem> ItemList = new List<IItem>()
    {
        new Gold(),
        new Coin(),
        new Key(),
        new Note(),
        new Lore()
    };

    public List<IItem> DecoratedItemList = new List<IItem>()
    {
        new LuckItemDecorator(new Key()),
        new LuckItemDecorator(new Gold()),
        new LuckItemDecorator(new Coin()),
        new LuckItemDecorator(new LuckItemDecorator(new Lore())),
        new LuckItemDecorator(new LuckItemDecorator (new Coin())),
    };

    public List<IItem> WeaponList = new List<IItem>()
    {
        new Hammer(),
        new Sword(),
        new Dagger(),
    };

    public List<IItem> DecoratedWeaponList = new List<IItem>()
    {
        new LuckItemDecorator(new Sword()),
        new PowerWeaponDecorator(new Hammer()),
        new PowerWeaponDecorator(new PowerWeaponDecorator(new Hammer())),
        new LuckItemDecorator( new PowerWeaponDecorator(new PowerWeaponDecorator(new Hammer()))),
        new Hammer(),
        new Sword(),
        new Dagger(),
        new PowerWeaponDecorator(new Dagger())
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

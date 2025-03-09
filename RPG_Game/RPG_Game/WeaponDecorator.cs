using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game;

//Decorator which is specific for a weapon
public class WeaponDecorator : IWeapon
{
    protected IWeapon weapon;
    public int Capacity => weapon.Capacity;
    public virtual int Damage => weapon.Damage;
    public virtual string Name => weapon.Name;

    public WeaponDecorator(IWeapon weapon)
    {
        this.weapon = weapon;
    }
}

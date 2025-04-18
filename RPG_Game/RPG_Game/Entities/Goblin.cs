using RPG_Game.Controller;
using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public class Goblin : IEnemy
{
    public event EventHandler? EntityMoved;
    public (int x, int y) Position { get; set; }
    private EnemyStats goblinStats = new EnemyStats();
    public AttackStrategy AttackStrategy { get; set; } = new NormalAttackStrategy();
    public IWeapon Weapon = new PowerWeaponDecorator(new Sword());
    public bool Move(Direction direction, Room room)
    {
        throw new NotImplementedException();
    }
    public void Attack(IEntity target)
    {
        Weapon.Use(AttackStrategy, this, new List<IEntity>() { target });
    }
    public void ReceiveDamage(int damage, IEntity? source)
    {
        goblinStats.ModifyEntityAttribute(PlayerAttributes.Health, -damage);
        if (source != null)
            Attack(source);
    }
    public Goblin() { }

    public object Copy()
    {
        return new Goblin();
    }

    public override string ToString() => "Goblin";

    public EntityStats RetrieveEntityStats() => this.goblinStats;
}

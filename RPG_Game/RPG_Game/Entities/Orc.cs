using RPG_Game.Controller;
using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.UIHandlers;
using RPG_Game.Weapons;
using RPG_Game.LogMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Entities;

public class Orc : IEnemy
{
    public event EventHandler? EntityMoved;
    public Position Position { get; set; }
    private EnemyStats orcStats = new EnemyStats();
    public IWeapon Weapon = new PowerWeaponDecorator(new Hammer());
    public AttackStrategy AttackStrategy { get; set; } = new NormalAttackStrategy();
    public event EventHandler? OwnDeath;
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
        orcStats.ModifyEntityAttribute(PlayerAttributes.Health, -damage);
        if (source != null)
        {
            ConsoleView.GetInstance().LogMessage(new OnAttackMessage(source, this, damage));
            ConsoleView.GetInstance().LogMessage(new OnEnemyDetectionMessage(this));
            Attack(source);
        }
    }
    public Orc() 
    {
        this.orcStats.Died += OwnDeathHandler;
    }
    public override string ToString() => "Orc";
    public EntityStats GetEntityStats() => this.orcStats;
    public object Copy()
    {
        return new Orc();
    }
    protected void OwnDeathHandler(object sender, EventArgs e)
    {
        this.OwnDeath?.Invoke(this, EventArgs.Empty);
    }
}

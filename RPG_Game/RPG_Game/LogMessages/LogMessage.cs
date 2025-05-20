using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.Model;
using RPG_Game.Model.Entities;
using RPG_Game.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.LogMessages;

public abstract class LogMessage : ILogMessage
{ 
    public IServerView ServerView { get; protected set; }
    public int PlayerId { get; }
    public abstract void Send();
    public void SetView(IServerView serverView)
    {
        ServerView = serverView;
    }
    public LogMessage(int playerId)
    {
        PlayerId = playerId;
    }
}

public class AttackLogMessage : LogMessage
{
    public Entity Source { get; }
    public Entity Target { get; }
    public int Damage { get; }
    public AttackLogMessage(int playerId, Entity source, Entity target, int damage): base(playerId)
    {
        this.Source = source;
        this.Target = target;
        Damage = damage;
    }
    public override void Send()
    {
        ServerView.LogMessage(this);
    }
}

public class EmptyInventoryLogMessage : LogMessage
{
    public EmptyInventoryLogMessage(int playerId) : base(playerId)
    {
    }
    public override void Send()
    {
        ServerView.LogMessage(this);
    }
}

public class EnemyDeathLogMessage : LogMessage
{
    public Entity Entity { get; }
    public EnemyDeathLogMessage(int playerId, Entity entity): base(playerId)
    {
        this.Entity = entity;
    }
    public override void Send()
    {
        ServerView.LogMessage(this);
    }
}

public class UseItemLogMessage : LogMessage
{
    public Item Item { get; }
    public UseItemLogMessage(int playerId, Item item): base(playerId)
    {
        this.Item = item;
    }
    public override void Send()
    {
        ServerView.LogMessage(this);
    }
}
public class DropItemLogMessage : UseItemLogMessage
{
    public DropItemLogMessage(int playerId, Item item) : base(playerId, item)
    {
    }
    public override void Send()
    {
        ServerView.LogMessage(this);
    }
}
public class ItemEquipLogMessage : UseItemLogMessage
{
    public ItemEquipLogMessage(int playerId, Item item) : base(playerId, item)
    {
    }
    public override void Send()
    {
        ServerView.LogMessage(this);
    }
}
public class ItemPickUpLogMessage : UseItemLogMessage
{
    public ItemPickUpLogMessage(int playerId, Item item) : base(playerId, item)
    {
    }
    public override void Send()
    {
        ServerView.LogMessage(this);
    }
}
public class ItemUnEquipLogMessage : UseItemLogMessage
{
    public ItemUnEquipLogMessage(int playerId, Item item) : base(playerId, item)
    {
    }
    public override void Send()
    {
        ServerView.LogMessage(this);
    }
}
public class PlayerMoveLogMessage : LogMessage
{
    public Direction Direction { get;}
    public PlayerMoveLogMessage(int playerId, Direction direction): base(playerId)
    {
        Direction = direction;
    }
    public override void Send()
    {
        ServerView.LogMessage(this);
    }
}
//public class PlayerDiedLogMessage: LogMessage
//{
//    public PlayerDiedLogMessage(int playerId) : base(playerId)
//    {
//    }
//    public override void Send()
//    {
//        ServerView.LogMessage(this);
//    }
//}

//public class EnemyDetectedLogMessage : LogMessage
//{
//    public Entity? Entity { get;}
//    public EnemyDetectedLogMessage(int playerId, Entity? entity): base(playerId)
//    {
//        this.Entity = entity;
//    }
//    public override void Send()
//    {
//        ServerView.LogMessage(this);
//    }
//}

//public class LogNotSupportedRequest : LogMessage
//{
//    public string Description => "The key is not supported";
//}

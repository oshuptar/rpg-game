using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.LogMessages;
using RPG_Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.View;

public class ServerConsoleView : ServerView
{
    public ServerConsoleView() : base(true)
    {
    }
    public override void LogMessage(EmptyInventoryLogMessage messageInfo)
    {
        LogMessage($"{messageInfo.PlayerId} emptied his inventory");
    }
    public override void LogMessage(PlayerMoveLogMessage messageInfo)
    {
        LogMessage($"{messageInfo.PlayerId} moved to the {messageInfo.Direction}");
    }

    public override void LogMessage(ItemUnEquipLogMessage messageInfo)
    {
        LogMessage($"{messageInfo.PlayerId} unequipped {messageInfo.Item.Name}");
    }

    public override void LogMessage(ItemEquipLogMessage messageInfo)
    {
        LogMessage($"{messageInfo.PlayerId} equipped {messageInfo.Item.Name} {messageInfo.Item.Description}");
    }

    public override void LogMessage(DropItemLogMessage messageInfo)
    {
        LogMessage($"{messageInfo.PlayerId} dropped {messageInfo.Item.Name}");
    }

    public override void LogMessage(ItemPickUpLogMessage messageInfo)
    {
        LogMessage($"{messageInfo.PlayerId} picked up {messageInfo.Item.Name} {messageInfo.Item.Description}");
    }

    //public override void LogMessage(OnRequestNotSupportedMessage messageInfo)
    //{
    //    LogMessage($"{messageInfo.Description}");
    //}

    public override void LogMessage(EnemyDeathLogMessage messageInfo)
    {
        LogMessage($"{messageInfo.Entity.ToString()} was killed");
    }

    public override void LogMessage(AttackLogMessage messageInfo)
    {
        LogMessage($"{messageInfo.Source} attacked {messageInfo.Target}: -{messageInfo.Damage}HP");
    }
    public override void LogMessage(string message)
    {
        Console.WriteLine($"[Server] player ID:{message}");
    }

    public override void LogMessage(UseItemLogMessage messageInfo)
    {
        LogMessage($"{messageInfo.PlayerId} used {messageInfo.Item}: -{messageInfo.Item.Description}");
    }
    public override void ServerStop()
    {
        base.ServerStop();
        Console.WriteLine("[Server] Server is terminating...");
    }
}

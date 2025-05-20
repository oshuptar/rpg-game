using RPG_Game.Controller;
using RPG_Game.Enums;
using RPG_Game.LogMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.View;

// Divide the views into two different categories, move input and reading input into a separate interface
public interface IServerView : IView
{
    public void LogMessage(EmptyInventoryLogMessage messageInfo);
    public void LogMessage(PlayerMoveLogMessage messageInfo);
    public void LogMessage(ItemUnEquipLogMessage messageInfo);
    public void LogMessage(ItemEquipLogMessage messageInfo);
    public void LogMessage(DropItemLogMessage messageInfo);
    public void LogMessage(ItemPickUpLogMessage messageInfo);
    public void LogMessage(EnemyDeathLogMessage messageInfo);
    public void LogMessage(AttackLogMessage messageInfo);
    public void LogMessage(UseItemLogMessage messageInfo);
    public void LogMessage(string message);
    public void ServerStop();
}



public abstract class ServerView : View, IServerView
{
    protected ServerView(bool flag) : base(true)
    {
    }
    public override void ReadInput(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            RequestType? requestType = GameInputHandler.TranslateRequest();
            if (requestType != null)
            {
                new Request(
                    requestType.Value,
                    Controller).SendRequest();
            }
        }
    }
    public virtual void ServerStop()
    {
        CancellationTokenSource.Cancel();
    }
    public abstract void LogMessage(EmptyInventoryLogMessage messageInfo);
    public abstract void LogMessage(PlayerMoveLogMessage messageInfo);
    public abstract void LogMessage(ItemUnEquipLogMessage messageInfo);
    public abstract void LogMessage(ItemEquipLogMessage messageInfo);
    public abstract void LogMessage(DropItemLogMessage messageInfo);
    public abstract void LogMessage(ItemPickUpLogMessage messageInfo);
    //public abstract void LogMessage(OnRequestNotSupportedMessage messageInfo);
    public abstract void LogMessage(EnemyDeathLogMessage messageInfo);
    public abstract void LogMessage(AttackLogMessage messageInfo);
    public abstract void LogMessage(string message);
    public abstract void LogMessage(UseItemLogMessage messageInfo);
}

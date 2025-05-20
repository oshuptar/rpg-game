using RPG_Game.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Entities;
using RPG_Game.Model;
using RPG_Game.Enums;
using RPG_Game.View;
using RPG_Game.LogMessages;

namespace RPG_Game.UIHandlers;

public interface IView
{
    //public void WelcomeRoutine();
    //public void DisplayRoutine();
    //public void EndRoutine(bool flag);
    public void SetController(IController controller);
    public void SetGameState(IGameState gameState);
    public void ReadInput(CancellationToken cancellationToken);
    //public void SetInventoryFocus();
    //public void SetHandsFocus();
    //public void ResetFocusType();
    //public void ShiftCurrentFocus(Direction direction);
    public void SendCommand(IViewCommand command);
    public void HandleCommand();
    //public RequestType? TranslateRequest();
    //public void LogMessage(OnEmptyDirectory messageInfo);
    //public void LogMessage(OnMoveMessage messageInfo);
    //public void LogMessage(OnItemUnequipMessage messageInfo);
    //public void LogMessage(OnItemEquipMessage messageInfo);
    //public void LogMessage(OnItemDropMessage messageInfo);
    //public void LogMessage(OnItemPickUpMessage messageInfo);
    //public void LogMessage(OnRequestNotSupportedMessage messageInfo);
    //public void LogMessage(OnEnemyDeathMessage messageInfo);
    //public void LogMessage(OnAttackMessage messageInfo);
}

//public interface ILogClear
//{
//    public void ClearLogMessage();
//}

//public interface IInputHandler
//{
//    public RequestType? TranslateRequest();
//}

//public interface HideControls
//{
//    public void HideControls();
//}
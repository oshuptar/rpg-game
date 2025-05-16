using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.UIHandlers;
using RPG_Game.LogMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Entities;

namespace RPG_Game.Controller;

public class BaseHandler : IRequestHandler
{
    protected virtual RequestType RequestType => RequestType.Ignore;
    protected IRequestHandler? NextHandler { get; set; } = null;


    public virtual void HandleRequest(ActionRequest request)
    {
        NextHandler?.HandleRequest(request);
    }
    public virtual bool CanHandleRequest(ActionRequest request)
    {
        if (request.GetActionRequestType() == RequestType)
            return true;
        return false;
    }
    public virtual void SetNext(IRequestHandler? controller)
    {
        NextHandler = controller;
    }
}

public class DefaultHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.Ignore;

    public override void HandleRequest(ActionRequest request)
    {
        ClientConsoleView.GetInstance().ClearLogMessage();
        ClientConsoleView.GetInstance().LogMessage(new OnRequestNotSupportedMessage());
    }
}

public class NormalAttackModeHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.NormalAttack;
    public override void HandleRequest(ActionRequest request)
    {
         if (CanHandleRequest(request)) 
            request.GetContext().GetGame().SetAttackMode(AttackType.NormalAttack, new NormalAttackStrategy());
        else
            base.HandleRequest(request);
    }
}
public class StealthAttackModeHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.StealthAttack;

    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
            request.GetContext().GetGame().SetAttackMode(AttackType.StealthAttack, new StealthAttackStrategy());
        else
            base.HandleRequest(request);
    }
}
public class MagicAttackModeHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.MagicAttack;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
            request.GetContext().GetGame().SetAttackMode(AttackType.MagicAttack, new MagicAttackStrategy());
        else
            base.HandleRequest(request);
    }
}
public class OneWeaponAttackHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.OneWeaponAttack;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            Context gameContext = request.GetContext();
            if(ClientConsoleView.GetInstance().FocusOn == FocusType.Hands)
            {
                IWeapon? weapon = gameContext.GetPlayer().GetHands().GetHandState().Hands[ClientConsoleView.GetInstance().CurrentFocus] as IWeapon;
                if (weapon == null) return;

                List<IEntity>? entities = gameContext.GetRoom().RetrieveEnemiesInRadius(request.GetContext().GetPlayer(), weapon.RadiusOfAction);
                weapon?.Use(request.GetContext().GetGame().AttackStrategy, gameContext.GetPlayer(), entities);
            }
        }
        else
            base.HandleRequest(request);
    }
}

public class TwoWeaponAttackHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.TwoWeaponAttack;

    public override void HandleRequest(ActionRequest request)
    {
        if(CanHandleRequest(request))
        {
            // Ask how to fix too nested calls
            foreach(var item in request.GetContext().GetPlayer().GetHands().GetHandState().Hands)
            {
                Context gameContext = request.GetContext();

                IWeapon? weapon = item as IWeapon;
                if (weapon == null) return;

                List<IEntity>? entities = gameContext.GetRoom().RetrieveEnemiesInRadius(request.GetContext().GetPlayer(), weapon.RadiusOfAction);
                weapon?.Use(request.GetContext().GetGame().AttackStrategy, gameContext.GetPlayer(), entities);
            }
        }
        else
            base.HandleRequest(request);
    }
}

public class UseItemHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.UseItem;

    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            if (ClientConsoleView.GetInstance().FocusOn == FocusType.Room)
                return;

            Context gameContext = request.GetContext();
            IItem? item = gameContext.GetPlayer().Retrieve(ClientConsoleView.GetInstance().CurrentFocus, 
                ClientConsoleView.GetInstance().FocusOn == FocusType.Inventory);

            item?.Use(request.GetContext().GetGame().AttackStrategy, gameContext.GetPlayer(), null);
        }
        else
            base.HandleRequest(request);
    }
}

// will be implemented with events later on, so the code common part would be smaller
//Figure out how to clear out LogMessages

//The displaying could be implemented via events in the view
// Think about it, or could explicitly delegated by the controller

public class MoveUpHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.MoveUp;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            ClientConsoleView.GetInstance().ClearLogMessage();
            Context gameContext = request.GetContext();
            gameContext.GetPlayer().Move(Direction.North, gameContext.GetRoom());
            ClientConsoleView.GetInstance().ResetFocusIndex();
        }
        else
            base.HandleRequest(request);
    }
}

public class MoveDownHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.MoveDown;

    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            ClientConsoleView.GetInstance().ClearLogMessage();
            Context gameContext = request.GetContext();
            gameContext.GetPlayer().Move(Direction.South, gameContext.GetRoom());
            ClientConsoleView.GetInstance().ResetFocusIndex();
        }
        else
            base.HandleRequest(request);
    }
}


public class MoveRightHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.MoveRight;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            ClientConsoleView.GetInstance().ClearLogMessage();
            Context gameContext = request.GetContext();
            gameContext.GetPlayer().Move(Direction.East, gameContext.GetRoom());
            ClientConsoleView.GetInstance().ResetFocusIndex();
        }
        else
            base.HandleRequest(request);
    }
}

public class MoveLeftHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.MoveLeft;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            ClientConsoleView.GetInstance().ClearLogMessage();
            Context gameContext = request.GetContext();
            gameContext.GetPlayer().Move(Direction.West, gameContext.GetRoom());
            ClientConsoleView.GetInstance().ResetFocusIndex();
        }
        else
            base.HandleRequest(request);
    }
}

public class QuitHadler : BaseHandler
{
    protected override RequestType RequestType => RequestType.Quit;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            // TODO: save state logic can be implemented
            // TODO: Add confirmation logic
            ClientConsoleView.GetInstance().ClearLogMessage();
            ClientConsoleView.GetInstance().ClearConsole();
            Thread.Sleep(1000);
            Environment.Exit(0);
        }
        else
            base.HandleRequest(request);
    }
}


public class PickUpItemHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.PickUpItem;

    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            if (ClientConsoleView.GetInstance().FocusOn == FocusType.Room)
            {
                ClientConsoleView.GetInstance().ClearLogMessage();
                Context gameContext = request.GetContext();
                IItem? item = gameContext.GetRoom().RemoveItem(gameContext.GetPlayer().Position, ClientConsoleView.GetInstance().CurrentFocus);
                gameContext.GetPlayer().PickUp(item);
            }
        }
        else
            base.HandleRequest(request);
    }
}

public class DropItemHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.DropItem;

    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            ClientConsoleView.GetInstance().ClearLogMessage();

            Context gameContext = request.GetContext();
            if (ClientConsoleView.GetInstance().FocusOn == FocusType.Inventory)
            {
                gameContext.GetPlayer().Drop(gameContext.GetRoom(), ClientConsoleView.GetInstance().CurrentFocus, true);
                ClientConsoleView.GetInstance().ShiftCurrentFocus(Direction.East);
            }
            else if (ClientConsoleView.GetInstance().FocusOn == FocusType.Hands)
            {
                gameContext.GetPlayer().Drop(gameContext.GetRoom(), ClientConsoleView.GetInstance().CurrentFocus, false);
                ClientConsoleView.GetInstance().ShiftCurrentFocus(Direction.East);
            }
        }
        else
            base.HandleRequest(request);
    }
}
public class EquipItemHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.EquipItem;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            ClientConsoleView.GetInstance().ClearLogMessage();

            Context gameContext = request.GetContext();
            Player player = gameContext.GetPlayer();
            ClientConsoleView displayer = ClientConsoleView.GetInstance();
            Room room = gameContext.GetRoom();
            IItem? item = null;

            switch (ClientConsoleView.GetInstance().FocusOn)
            {
                case FocusType.Inventory:
                    item = player.Retrieve(displayer.CurrentFocus, true);
                    if (player.Equip(item))
                        item = player.Remove(displayer.CurrentFocus, true);
                    break;
                // Unequips
                case FocusType.Hands:
                    player.UnEquip(displayer.CurrentFocus);
                    break;
                case FocusType.Room:
                    item = room.RemoveItem(player.Position, displayer.CurrentFocus);
                    if (!player.Equip(item, false))
                        room.AddItem(item, player.Position);
                    break;
            }
        }
        else
            base.HandleRequest(request);
    }
}
public class EmptyInventoryHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.EmptyInventory;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            if (ClientConsoleView.GetInstance().FocusOn == FocusType.Inventory)
            {
                Context gameContext = request.GetContext();
                gameContext.GetPlayer().EmptyInventory(gameContext.GetRoom());
                ClientConsoleView.GetInstance().ResetFocusType();
            }
        }
        else
            base.HandleRequest(request);
    }
}
public class ScopeInventoryHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.ScopeInventory;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            ClientConsoleView.GetInstance().ClearLogMessage();
            ClientConsoleView.GetInstance().SetInventoryFocus();
        }
        else
            base.HandleRequest(request);
    }
}

public class ScopeHandsHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.ScopeHands;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            ClientConsoleView.GetInstance().ClearLogMessage();
            ClientConsoleView.GetInstance().SetHandsFocus();
        }
        else
            base.HandleRequest(request);
    }
}

public class ScopeRoomHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.ScopeRoom;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            ClientConsoleView.GetInstance().ClearLogMessage();
            ClientConsoleView.GetInstance().ResetFocusType();
        }
        else
            base.HandleRequest(request);
    }
}

public class HideControlsHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.HideControls;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            ClientConsoleView.GetInstance().ClearLogMessage();
            ClientConsoleView.GetInstance().ChangeControlsVisibility();
        }
        else
            base.HandleRequest(request);
    }
}

public class NextItemHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.NextItem;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            ClientConsoleView.GetInstance().ClearLogMessage();
            ClientConsoleView.GetInstance().ShiftCurrentFocus(Direction.East);
        }
        else
            base.HandleRequest(request);
    }
}

public class PrevItemHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.PrevItem;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            ClientConsoleView.GetInstance().ClearLogMessage();
            ClientConsoleView.GetInstance().ShiftCurrentFocus(Direction.West);
        }
        else
            base.HandleRequest(request);
    }
}




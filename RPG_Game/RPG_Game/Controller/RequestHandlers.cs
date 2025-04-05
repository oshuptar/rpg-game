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

// will be implemented with events later on, so the code common part would be smaller
public class MoveUpHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.MoveUp;
    public override void HandleRequest(ActionRequest request)
    {
        if (CanHandleRequest(request))
        {
            //MovePlayerAction(request, Direction.North);
            Context gameContext = request.GetContext();
            gameContext.GetPlayer().Move(Direction.North, gameContext.GetRoom());
            ConsoleObjectDisplayer.GetInstance().ResetFocusIndex();
            //The displaying could be implemented via events in the view
            // Think about it, or could explicitly delegated by the controller
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
            Context gameContext = request.GetContext();
            gameContext.GetPlayer().Move(Direction.South, gameContext.GetRoom());
            ConsoleObjectDisplayer.GetInstance().ResetFocusIndex();
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
            Context gameContext = request.GetContext();
            gameContext.GetPlayer().Move(Direction.East, gameContext.GetRoom());
            ConsoleObjectDisplayer.GetInstance().ResetFocusIndex();
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
            Context gameContext = request.GetContext();
            gameContext.GetPlayer().Move(Direction.West, gameContext.GetRoom());
            ConsoleObjectDisplayer.GetInstance().ResetFocusIndex();
        }
        else
            base.HandleRequest(request);
    }
}

// default handler
public class DefaultHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.Ignore;

    public override void HandleRequest(ActionRequest request)
    {
        ConsoleObjectDisplayer.GetInstance().LogMessage(new OnRequestNotSupportedMessage());
    }
}


public class PickUpItemHandler : BaseHandler
{
    protected override RequestType RequestType => RequestType.PickUpItem;

    public override void HandleRequest(ActionRequest request)
    {
        if(CanHandleRequest(request))
        {
            Context gameContext = request.GetContext();
            IItem? item = gameContext.GetRoom().RemoveItem(gameContext.GetPlayer().Position, ConsoleObjectDisplayer.GetInstance().CurrentFocus);
            gameContext.GetPlayer().PickUp(item);
            ConsoleObjectDisplayer.GetInstance().ResetFocusIndex();
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
        if(CanHandleRequest(request))
        {
            Context gameContext = request.GetContext();
            if (ConsoleObjectDisplayer.GetInstance().FocusOn == FocusType.Inventory)
            {
                gameContext.GetPlayer().Drop(gameContext.GetRoom(), ConsoleObjectDisplayer.GetInstance().CurrentFocus, true);
                ConsoleObjectDisplayer.GetInstance().ResetFocusIndex();
            }
            else if (ConsoleObjectDisplayer.GetInstance().FocusOn == FocusType.Hands)
            {
                gameContext.GetPlayer().Drop(gameContext.GetRoom(), ConsoleObjectDisplayer.GetInstance().CurrentFocus, false);
                ConsoleObjectDisplayer.GetInstance().ResetFocusIndex();
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
            Context gameContext = request.GetContext();
            Player player = gameContext.GetPlayer();
            ConsoleObjectDisplayer displayer = ConsoleObjectDisplayer.GetInstance();
            Room room = gameContext.GetRoom();
            IItem? item = null;

            switch (ConsoleObjectDisplayer.GetInstance().FocusOn)
            {
                case FocusType.Inventory:
                    item = player.Retrieve(displayer.CurrentFocus, true);
                    if (player.Equip(item))
                        item = player.Remove(displayer.CurrentFocus, true);
                    displayer.ResetFocusIndex();
                    break;
                // Unequips
                case FocusType.Hands:
                    player.UnEquip(displayer.CurrentFocus);
                    displayer.ResetFocusIndex();
                    break;
                case FocusType.Room:
                    item = room.RemoveItem(player.Position, displayer.CurrentFocus);
                    if (!player.Equip(item, false))
                        room.AddItem(item, player.Position);
                    displayer.ResetFocusIndex();
                    break;
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
            ConsoleObjectDisplayer.GetInstance().SetInventoryFocus();
            ConsoleObjectDisplayer.GetInstance().ResetFocusIndex();
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
        if(CanHandleRequest(request))
        {
            ConsoleObjectDisplayer.GetInstance().SetHandsFocus();
            ConsoleObjectDisplayer.GetInstance().ResetFocusIndex();
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
            ConsoleObjectDisplayer.GetInstance().ResetFocusType();
            ConsoleObjectDisplayer.GetInstance().ResetFocusIndex();
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
            ConsoleObjectDisplayer.GetInstance().ChangeControlsVisibility();
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
            ConsoleObjectDisplayer.GetInstance().ShiftCurrentFocus(request.GetContext().GetPlayer(), Direction.East);
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
            ConsoleObjectDisplayer.GetInstance().ShiftCurrentFocus(request.GetContext().GetPlayer(), Direction.West);
        else
            base.HandleRequest(request);
    }
}






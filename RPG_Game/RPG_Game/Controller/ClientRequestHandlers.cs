using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.LogMessages;
using RPG_Game.Model;
using RPG_Game.UIHandlers;
using RPG_Game.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ClientRequestHandler : IRequestHandler
{
    protected virtual RequestType RequestType => RequestType.Ignore;
    protected IRequestHandler? NextHandler { get; set; } = null;
    public virtual List<IViewCommand>? HandleRequest(Request request)
    {
        return NextHandler?.HandleRequest(request);
    }
    public virtual bool CanHandleRequest(Request request)
    {
        if (request.RequestType == RequestType)
            return true;
        return false;
    }
    public virtual void SetNext(IRequestHandler? controller)
    {
        NextHandler = controller;
    }
}
public class DefaultHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.Ignore;

    public override List<IViewCommand>? HandleRequest(Request request)
    {
        // Should notify the user that the key is not supported
        return null;
    }
}
public class ScopeInventoryHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.ScopeInventory;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
            return new List<IViewCommand>() {  new InventoryFocusCommand() };
        else
            return base.HandleRequest(request);
    }
}
public class ScopeHandsHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.ScopeHands;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
            return new List<IViewCommand>() { new HandsFocusCommand() };
        else
            return base.HandleRequest(request);
    }
}
public class ScopeRoomHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.ScopeRoom;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
            return new List<IViewCommand>() { new RoomFocusCommand() };
        else
            return base.HandleRequest(request);
    }
}
public class HideControlsHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.HideControls;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
            return new List<IViewCommand>() { new HideControlsCommand() };
        else
            return base.HandleRequest(request);
    }
}
public class NextItemHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.NextItem;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
            return new List<IViewCommand>() { new ShiftFocusCommand(Direction.East) };
        else
            return base.HandleRequest(request);
    }
}
public class PrevItemHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.PrevItem;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
            return new List<IViewCommand>() { new ShiftFocusCommand(Direction.West) };
        else
            return base.HandleRequest(request);
    }
}

public class NormalAttackModeClientHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.NormalAttack;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        // Modification of state predictively
        if (CanHandleRequest(request))
        {
            //request.GameState.GetPlayer().SetAttackMode(AttackType.NormalAttack, new NormalAttackStrategy());
            ClientHandlerChain.GetInstance().ClientController.SendNetworkRequest(request);
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class StealthAttackModeClientHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.StealthAttack;

    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            //request.GameState.GetPlayer().SetAttackMode(AttackType.StealthAttack, new StealthAttackStrategy());
            ClientHandlerChain.GetInstance().ClientController.SendNetworkRequest(request);
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class MagicAttackModeClientHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.MagicAttack;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            //request.GameState.GetPlayer().SetAttackMode(AttackType.MagicAttack, new MagicAttackStrategy());
            ClientHandlerChain.GetInstance().ClientController.SendNetworkRequest(request);
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class OneWeaponAttackClientHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.OneWeaponAttack;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            if (request.FocusOn == FocusType.Hands)
            {
                // Retrieve player Id and get the weapon focused
                ClientController clientController = ClientHandlerChain.GetInstance().ClientController;
                //clientController.
                //IWeapon? weapon = request.GameState;
                //.GetPlayer().GetHands().GetHandState().Hands[request.CurrentFocus] as IWeapon;
                //List<IEntity>? entities = gameContext.GetRoom().RetrieveEntitiesInRadius(request.GetContext().GetPlayer(), weapon.RadiusOfAction);
                //weapon?.Use(request.GetContext().GetGame().AttackStrategy, gameContext.GetPlayer(), entities);

                Player player = request.GameState.GetPlayer();
                // Make it more safe
                Weapon? weapon = player.GetHands().GetHandState().Hands[request.CurrentFocus] as Weapon;
                ItemActionRequest itemRequest = ItemActionRequest.CreateItemActionRequest(request);
                if(weapon == null) return null;
                itemRequest.Items.Add(weapon);

                // Serialize and send request
                ClientHandlerChain.GetInstance().ClientController.SendNetworkRequest(itemRequest);
            }
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class TwoWeaponAttackClientHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.TwoWeaponAttack;

    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            ItemActionRequest itemRequest = ItemActionRequest.CreateItemActionRequest(request);
            foreach (var item in player.GetHands().GetHandState().Hands)
            {
                Weapon? weapon = item as Weapon;
                if (weapon != null) itemRequest.Items.Add(weapon);
            }
            ClientHandlerChain.GetInstance().ClientController.SendNetworkRequest(itemRequest);
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class UseItemClientHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.UseItem;

    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            if (request.FocusOn == FocusType.Room)
                return null;

            ItemActionRequest itemRequest = ItemActionRequest.CreateItemActionRequest(request);
            Player player = request.GameState.GetPlayer();
            Item? item = player.Retrieve(request.CurrentFocus, request.FocusOn == FocusType.Inventory);
            if(item != null) itemRequest.Items.Add(item);

            ClientHandlerChain.GetInstance().ClientController.SendNetworkRequest(itemRequest);
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class MoveUpClientHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.MoveUp;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            ClientHandlerChain.GetInstance().ClientController.SendNetworkRequest(request);
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class MoveDownClientHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.MoveDown;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            ClientHandlerChain.GetInstance().ClientController.SendNetworkRequest(request);
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class MoveRightClientHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.MoveRight;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            ClientHandlerChain.GetInstance().ClientController.SendNetworkRequest(request);
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class MoveLeftClientHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.MoveLeft;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            ClientHandlerChain.GetInstance().ClientController.SendNetworkRequest(request);
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class QuitClientHadler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.Quit;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
            return new List<IViewCommand>() { new QuitCommand(true) };
        else
            return base.HandleRequest(request);
    }
}
public class PickUpItemClientHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.PickUpItem;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            if (request.FocusOn == FocusType.Room)
            {
                Player player = request.GameState.GetPlayer();
                ItemActionRequest itemRequest = ItemActionRequest.CreateItemActionRequest(request);
                List<Item>? items = request.GameState.GetItems(player.Position);
                if(items == null) return null;
                Item item = items[request.CurrentFocus];
                itemRequest.Items.Add(item);
                ClientHandlerChain.GetInstance().ClientController.SendNetworkRequest(itemRequest);
            }
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class DropItemClientHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.DropItem;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            //ClientConsoleView.GetInstance().ClearLogMessage();
            Player player = request.GameState.GetPlayer();
            ItemActionRequest itemRequest = ItemActionRequest.CreateItemActionRequest(request);
            if (request.FocusOn == FocusType.Room) return null;
            Item? item = player.Retrieve(request.CurrentFocus, request.FocusOn == FocusType.Inventory);

            if (item == null) return null;
            itemRequest.Items.Add(item);

            ClientHandlerChain.GetInstance().ClientController.SendNetworkRequest(itemRequest);
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class EquipItemClientHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.EquipItem;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            ItemActionRequest itemRequest = ItemActionRequest.CreateItemActionRequest(request);
            Item? item;
            switch (request.FocusOn)
            {
                case FocusType.Room:
                    List<Item>? items = request.GameState.GetItems(player.Position);
                    if (items == null) return null;
                    item = items[request.CurrentFocus];
                    //itemRequest.Items.Add(item);
                break;
                default:
                    item = player.Retrieve(request.CurrentFocus, request.FocusOn == FocusType.Inventory);
                    break;
            }
            if (item == null) return null;
            itemRequest.Items.Add(item);
            ClientHandlerChain.GetInstance().ClientController.SendNetworkRequest(itemRequest);
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class EmptyInventoryClientHandler : ClientRequestHandler
{
    protected override RequestType RequestType => RequestType.EmptyInventory;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            if(request.FocusOn == FocusType.Inventory)
                ClientHandlerChain.GetInstance().ClientController.SendNetworkRequest(request);
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
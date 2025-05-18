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
using RPG_Game.Model;
using RPG_Game.View;
using System.ComponentModel;
using RPG_Game.Model.Entities;

namespace RPG_Game.Controller;

public class ServerRequestHandler : IRequestHandler
{
    protected virtual RequestType RequestType => RequestType.Ignore;
    protected IRequestHandler? NextHandler { get; set; } = null;
    public virtual List<IViewCommand>? HandleRequest(Request request)
    {
        NextHandler?.HandleRequest(request);
        return null;
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

public class UseItemServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.UseItem;

    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            ItemActionRequest itemActionRequest = (ItemActionRequest)request;
            List<Item> items = itemActionRequest.Items;

            if (items.Count == 0) return null;
            Item? item = player.Retrieve(request.CurrentFocus,
                request.FocusOn == FocusType.Inventory);
            if (item != null && item.Equals(items[0]))
                item?.Use(player.AttackStrategy, player, null);

            // Perform some logging of the action

            new Response(RequestType.UseItem,
                request.PlayerId,
                ServerHandlerChain.GetInstance().ServerController.GetGameState()).HandleResponse();
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class MoveUpServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.MoveUp;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            player.Move(Direction.North, (Room)request.GameState); // think how to overcome this casting

            new Response(RequestType.UseItem,
                request.PlayerId,
                ServerHandlerChain.GetInstance().ServerController.GetGameState()).HandleResponse();
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class MoveDownServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.MoveDown;

    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            player.Move(Direction.South, (Room)request.GameState);

            new Response(RequestType.UseItem,
                request.PlayerId,
                ServerHandlerChain.GetInstance().ServerController.GetGameState()).HandleResponse();
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class MoveRightServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.MoveRight;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            player.Move(Direction.East, (Room)request.GameState);

            new Response(RequestType.UseItem,
                request.PlayerId,
                ServerHandlerChain.GetInstance().ServerController.GetGameState()).HandleResponse();
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class MoveLeftServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.MoveLeft;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            player.Move(Direction.West, (Room)request.GameState);

            new Response(RequestType.UseItem,
                request.PlayerId,
                ServerHandlerChain.GetInstance().ServerController.GetGameState()).HandleResponse();
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class QuitServerHadler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.Quit;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            // Handle player removal
            new Response(RequestType.UseItem,
                request.PlayerId,
                ServerHandlerChain.GetInstance().ServerController.GetGameState()).HandleResponse();
            return new List<IViewCommand>() { new QuitCommand(false) };
        }
        else
            return base.HandleRequest(request);
    }
}
public class PickUpItemServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.PickUpItem;

    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            ItemActionRequest itemActionRequest = (ItemActionRequest)request;
            List<Item> items = itemActionRequest.Items;
            if (items.Count == 0) return null;
            // Validation Logic

            Item? item = ((Room)request.GameState).RemoveItem(player.Position, items[0]);
            if (item == null) return null;
            player.PickUp(item);
            //Log action

            new Response(RequestType.UseItem,
                request.PlayerId,
                ServerHandlerChain.GetInstance().ServerController.GetGameState()).HandleResponse();
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class DropItemServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.DropItem;

    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            // Fix to remove by reference not by index
            Player player = request.GameState.GetPlayer();
            ItemActionRequest itemActionRequest = (ItemActionRequest)request;
            player.Drop((Room)request.GameState, itemActionRequest.Items[0], request.FocusOn == FocusType.Inventory);
            //player.Drop((Room)request.GameState, request.CurrentFocus, request.FocusOn == FocusType.Inventory);

            new Response(RequestType.UseItem,
                request.PlayerId,
                ServerHandlerChain.GetInstance().ServerController.GetGameState()).HandleResponse();
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class EquipItemServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.EquipItem;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            ItemActionRequest itemActionRequest = (ItemActionRequest)request;
            List<Item> items = itemActionRequest.Items;
            if (items.Count == 0) return null;

            Item? item = null;
            switch (request.FocusOn)
            {
                case FocusType.Inventory:
                    item = player.Retrieve(request.CurrentFocus, true);
                    if (player.Equip(item))
                        item = player.Remove(request.CurrentFocus, true);
                    break;
                case FocusType.Hands:
                    player.UnEquip(request.CurrentFocus);
                    break;
                case FocusType.Room:
                    Room room = ((Room)request.GameState);
                    item = room.RemoveItem(player.Position, items[0]);
                    if (item == null || !item.Equals(items[0])) return null;
                    if (!player.Equip(item))
                        room.AddItem(item, player.Position);
                    break;
            }
            new Response(RequestType.UseItem,
                request.PlayerId,
                ServerHandlerChain.GetInstance().ServerController.GetGameState()).HandleResponse();
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class EmptyInventoryServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.EmptyInventory;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            player.EmptyInventory(((Room)request.GameState));

            new Response(RequestType.UseItem,
                request.PlayerId,
                ServerHandlerChain.GetInstance().ServerController.GetGameState()).HandleResponse();
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}

public class NormalAttackModeServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.NormalAttack;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            request.GameState.GetPlayer().SetAttackMode(AttackType.NormalAttack, new NormalAttackStrategy());
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class StealthAttackModeServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.StealthAttack;

    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            request.GameState.GetPlayer().SetAttackMode(AttackType.StealthAttack, new StealthAttackStrategy());
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class MagicAttackModeServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.MagicAttack;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            request.GameState.GetPlayer().SetAttackMode(AttackType.MagicAttack, new MagicAttackStrategy());
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}

public class OneWeaponAttackServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.OneWeaponAttack;
    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            ItemActionRequest itemActionRequest = (ItemActionRequest)request;

            foreach(Item item in itemActionRequest.Items)
            {
                if(player.GetHands().GetHandState().Hands.Contains(item))
                {
                    Weapon? weapon = item as Weapon;
                    if (weapon == null) return null;

                    List<Entity>? entities = ((Room)request.GameState).RetrieveEntitiesInRadius(player, weapon.RadiusOfAction);
                    weapon?.Use(player.AttackStrategy, player, entities);
                }
            }

            new Response(RequestType.UseItem,
                request.PlayerId,
                ServerHandlerChain.GetInstance().ServerController.GetGameState()).HandleResponse();
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class TwoWeaponAttackServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.TwoWeaponAttack;

    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            ItemActionRequest itemActionRequest = (ItemActionRequest)request;

            foreach (Item item in itemActionRequest.Items)
            {
                if (player.GetHands().GetHandState().Hands.Contains(item))
                {
                    Weapon? weapon = item as Weapon;
                    if (weapon == null) return null;

                    List<Entity>? entities = ((Room)request.GameState).RetrieveEntitiesInRadius(player, weapon.RadiusOfAction);
                    weapon?.Use(player.AttackStrategy, player, entities);
                }
            }

            new Response(RequestType.UseItem,
                request.PlayerId,
                ServerHandlerChain.GetInstance().ServerController.GetGameState()).HandleResponse();
            return null;
        }
        else
            return base.HandleRequest(request);
    }

}

public class ServerStopHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.ServerStop;

    public override List<IViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
            return new List<IViewCommand>() { new ServerStopCommand() };
        else
            return base.HandleRequest(request);
    }
}
using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
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

public class ServerRequestHandler : IServerRequestHandler
{
    protected virtual RequestType RequestType => RequestType.Ignore;
    protected IServerRequestHandler? NextHandler { get; set; } = null;
    public virtual List<IServerViewCommand>? HandleRequest(Request request)
    {
        return NextHandler?.HandleRequest(request);
    }
    public virtual bool CanHandleRequest(Request request)
    {
        if (request.RequestType == RequestType)
            return true;
        return false;
    }
    public virtual void SetNext(IServerRequestHandler? controller)
    {
        NextHandler = controller;
    }
}

public class UseItemServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.UseItem;

    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            ItemActionRequest itemActionRequest = (ItemActionRequest)request;
            List<Item> items = itemActionRequest.Items;

            if (items.Count == 0) return null;
            Item? item = player.Retrieve(request.CurrentFocus,
                request.FocusOn == FocusType.Inventory);
            if (item != null /*&& item.Equals(items[0])*/)
                item?.Use(player.AttackStrategy, player, null);

            // Perform some logging of the action

            Response response = new Response(RequestType.UseItem,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            if (item != null)
                return new List<IServerViewCommand>() { new LogCommand(new UseItemLogMessage(request.PlayerId, item)) };
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class MoveUpServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.MoveUp;
    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            player.Move(Direction.North, (AuthorityGameState)request.GameState); // think how to overcome this casting

            Response response = new Response(RequestType.MoveUp,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            return new List<IServerViewCommand>() { new LogCommand(new PlayerMoveLogMessage(request.PlayerId, Direction.North)) };
        }
        else
            return base.HandleRequest(request);
    }
}
public class MoveDownServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.MoveDown;

    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            // Unable to cast
            player.Move(Direction.South, (AuthorityGameState)request.GameState);

            Response response = new Response(RequestType.MoveDown,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            return new List<IServerViewCommand>() { new LogCommand(new PlayerMoveLogMessage(request.PlayerId, Direction.South)) };
        }
        else
            return base.HandleRequest(request);
    }
}
public class MoveRightServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.MoveRight;
    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            player.Move(Direction.East, (AuthorityGameState)request.GameState);

            Response response = new Response(RequestType.MoveRight,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            //return null;
            return new List<IServerViewCommand>() { new LogCommand(new PlayerMoveLogMessage(request.PlayerId, Direction.East)) };
        }
        else
            return base.HandleRequest(request);
    }
}
public class MoveLeftServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.MoveLeft;
    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            player.Move(Direction.West, (AuthorityGameState)request.GameState);

            Response response = new Response(RequestType.MoveLeft,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            return  new List<IServerViewCommand>() { new LogCommand(new PlayerMoveLogMessage(request.PlayerId, Direction.West)) };
        }
        else
            return base.HandleRequest(request);
    }
}
public class QuitServerHadler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.Quit;
    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            // Handle player removal
            Response response = new Response(RequestType.Quit,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            //return new List<IServerViewCommand>() { new QuitCommand(false) };
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class PickUpItemServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.PickUpItem;

    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            ItemActionRequest itemActionRequest = (ItemActionRequest)request;
            List<Item> items = itemActionRequest.Items;
            if (items.Count == 0) return null;
            // Validation Logic

            // Implement Value Checkers for types, or specify a unique ID to each item
            // Improve validation
            Item? item = ((AuthorityGameState)request.GameState).RemoveItem(player.Position, request.CurrentFocus);
            if (item == null) return null;
            player.PickUp(item);
            //Log action

            Response response = new Response(RequestType.PickUpItem,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            return new List<IServerViewCommand>() { new LogCommand(new ItemPickUpLogMessage(request.PlayerId, item)) };
        }
        else
            return base.HandleRequest(request);
    }
}
public class DropItemServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.DropItem;

    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            // Fix to remove by reference not by index
            Player player = request.GameState.GetPlayer();
            ItemActionRequest itemActionRequest = (ItemActionRequest)request;
            //player.Drop((Room)request.GameState, itemActionRequest.Items[0], request.FocusOn == FocusType.Inventory);
            Item? item = player.Drop((AuthorityGameState)request.GameState, request.CurrentFocus, request.FocusOn == FocusType.Inventory);
            if (item == null) return null;

            Response response = new Response(RequestType.DropItem,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            return new List<IServerViewCommand>() { new LogCommand(new DropItemLogMessage(request.PlayerId, item)) };
        }
        else
            return base.HandleRequest(request);
    }
}
public class EquipItemServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.EquipItem;
    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            List<IServerViewCommand> commands = new List<IServerViewCommand>();

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
                    return null;
                case FocusType.Hands:
                    player.UnEquip(request.CurrentFocus);
                    return null;
                case FocusType.Room:
                    AuthorityGameState room = ((AuthorityGameState)request.GameState);
                    item = room.RemoveItem(player.Position, request.CurrentFocus);
                    if (item == null /*|| !item.Equals(items[0])*/) return null;
                    if (!player.Equip(item))
                        room.AddItem(item, player.Position);
                    commands.Add(new LogCommand(new ItemPickUpLogMessage(request.PlayerId, item)));
                    break;
            }
            Response response = new Response(RequestType.EquipItem,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            return (commands.Count != 0) ? commands : null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class EmptyInventoryServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.EmptyInventory;
    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            player.EmptyInventory(((AuthorityGameState)request.GameState));

            Response response = new Response(RequestType.EmptyInventory,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            return new List<IServerViewCommand>() { new LogCommand(new EmptyInventoryLogMessage(request.PlayerId)) };
        }
        else
            return base.HandleRequest(request);
    }
}

public class NormalAttackModeServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.NormalAttack;
    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            ((AuthorityGameState)request.GameState).GetPlayer().SetAttackMode(AttackType.NormalAttack, new NormalAttackStrategy());
            Response response = new Response(RequestType.NormalAttack,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class StealthAttackModeServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.StealthAttack;

    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            ((AuthorityGameState)request.GameState).GetPlayer().SetAttackMode(AttackType.StealthAttack, new StealthAttackStrategy());
            Response response = new Response(RequestType.StealthAttack,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}
public class MagicAttackModeServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.MagicAttack;
    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            ((AuthorityGameState)request.GameState).GetPlayer().SetAttackMode(AttackType.MagicAttack, new MagicAttackStrategy());
            Response response = new Response(RequestType.MagicAttack,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            return null;
        }
        else
            return base.HandleRequest(request);
    }
}

public class OneWeaponAttackServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.OneWeaponAttack;
    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            ItemActionRequest itemActionRequest = (ItemActionRequest)request;

            List<IServerViewCommand> commands = new List<IServerViewCommand>();
            foreach (Item item in itemActionRequest.Items)
            {
                Weapon? weapon = item as Weapon;
                if (weapon == null) continue;

                List<Entity>? entities = ((AuthorityGameState)request.GameState).RetrieveEntitiesInRadius(player, weapon.RadiusOfAction);
                weapon?.Use(player.AttackStrategy, player, entities);
            }

            Response response = new Response(RequestType.OneWeaponAttack,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            return (commands.Count != 0) ? commands : null;
        }
        else
            return base.HandleRequest(request);
    }
}

//Implement logging
public class TwoWeaponAttackServerHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.TwoWeaponAttack;

    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
        {
            Player player = request.GameState.GetPlayer();
            ItemActionRequest itemActionRequest = (ItemActionRequest)request;

            List<IServerViewCommand> commands = new List<IServerViewCommand>();
            foreach (Item item in itemActionRequest.Items)
            {
                Weapon? weapon = item as Weapon;
                if (weapon == null) return null;

                List<Entity>? entities = ((AuthorityGameState)request.GameState).RetrieveEntitiesInRadius(player, weapon.RadiusOfAction);
                weapon?.Use(player.AttackStrategy, player, entities);
            }

            Response response = new Response(RequestType.TwoWeaponAttack,
                null,
                ServerHandlerChain.GetInstance().ServerController.GetGameState());
            response.Controller = ServerHandlerChain.GetInstance().ServerController;
            response.HandleResponse();
            return (commands.Count != 0) ? commands : null;
        }
        else
            return base.HandleRequest(request);
    }

}
public class ServerStopHandler : ServerRequestHandler
{
    protected override RequestType RequestType => RequestType.ServerStop;
    public override List<IServerViewCommand>? HandleRequest(Request request)
    {
        if (CanHandleRequest(request))
            return new List<IServerViewCommand>() { new ServerStopCommand() };
        else
            return base.HandleRequest(request);
    }
}
using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class KeyboardTranslator : IInputHandler
{
    // For not KeyBindings are static, but I will provide functionality to allow user anytime during the game to change them for the desired ones
    public Dictionary<ConsoleKey, RequestType> KeyBindings = new Dictionary<ConsoleKey, RequestType>();
    public RequestType? TranslateRequest()
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;

            RequestType requestType;
            if (!KeyBindings.TryGetValue(key, out requestType))
                requestType = RequestType.Ignore;

            return requestType;
        }
        return null;
    }

    public void DispatchRequest(ActionRequest request)
    { 
        RequestHandlerChain.GetInstance().HandleRequest(request);
    }

    // to exract the desired binding just read the key from console as usual
    public KeyboardTranslator()
    {
        KeyBindings.Add(ConsoleKey.W, RequestType.MoveUp);
        KeyBindings.Add(ConsoleKey.A, RequestType.MoveLeft);
        KeyBindings.Add(ConsoleKey.S, RequestType.MoveDown);
        KeyBindings.Add(ConsoleKey.D, RequestType.MoveRight);
        KeyBindings.Add(ConsoleKey.G, RequestType.DropItem);
        KeyBindings.Add(ConsoleKey.K, RequestType.HideControls);
        KeyBindings.Add(ConsoleKey.Q, RequestType.EquipItem);
        KeyBindings.Add(ConsoleKey.H, RequestType.ScopeHands);
        KeyBindings.Add(ConsoleKey.I, RequestType.ScopeInventory);
        KeyBindings.Add(ConsoleKey.Escape, RequestType.ScopeRoom);
        KeyBindings.Add(ConsoleKey.E, RequestType.PickUpItem);
        KeyBindings.Add(ConsoleKey.RightArrow, RequestType.NextItem);
        KeyBindings.Add(ConsoleKey.LeftArrow, RequestType.PrevItem);
    }
}

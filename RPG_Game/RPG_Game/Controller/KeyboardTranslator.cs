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
    public Dictionary<(ConsoleKey, ConsoleModifiers), RequestType> KeyBindings = new Dictionary<(ConsoleKey, ConsoleModifiers), RequestType>();
    public RequestType? TranslateRequest()
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true);
            RequestType requestType;
            if (!KeyBindings.TryGetValue((key.Key, key.Modifiers), out requestType))
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
        KeyBindings.Add((ConsoleKey.W, ConsoleModifiers.None), RequestType.MoveUp);
        KeyBindings.Add((ConsoleKey.A, ConsoleModifiers.None), RequestType.MoveLeft);
        KeyBindings.Add((ConsoleKey.S, ConsoleModifiers.None), RequestType.MoveDown);
        KeyBindings.Add((ConsoleKey.D, ConsoleModifiers.None), RequestType.MoveRight);
        KeyBindings.Add((ConsoleKey.G, ConsoleModifiers.None), RequestType.DropItem);
        KeyBindings.Add((ConsoleKey.G, ConsoleModifiers.Control), RequestType.EmptyInventory);
        KeyBindings.Add((ConsoleKey.K, ConsoleModifiers.None), RequestType.HideControls);
        KeyBindings.Add((ConsoleKey.Q, ConsoleModifiers.None), RequestType.EquipItem);
        KeyBindings.Add((ConsoleKey.H, ConsoleModifiers.None), RequestType.ScopeHands);
        KeyBindings.Add((ConsoleKey.I, ConsoleModifiers.None), RequestType.ScopeInventory);
        KeyBindings.Add((ConsoleKey.Escape, ConsoleModifiers.None), RequestType.ScopeRoom);
        KeyBindings.Add((ConsoleKey.E, ConsoleModifiers.None), RequestType.PickUpItem);
        KeyBindings.Add((ConsoleKey.RightArrow, ConsoleModifiers.None), RequestType.NextItem);
        KeyBindings.Add((ConsoleKey.LeftArrow, ConsoleModifiers.None), RequestType.PrevItem);
        KeyBindings.Add((ConsoleKey.X, ConsoleModifiers.None), RequestType.Quit);
    }
}

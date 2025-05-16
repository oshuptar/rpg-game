using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class KeyboardTranslator : IInputHandler
{
    public Dictionary<(ConsoleKey, ConsoleModifiers), RequestType> KeyBindings = new Dictionary<(ConsoleKey, ConsoleModifiers), RequestType>();
    public RequestType? TranslateRequest()
    {
        if (!Console.KeyAvailable)
            return null;

        var key = Console.ReadKey(true);
        if (!KeyBindings.TryGetValue((key.Key, key.Modifiers), out RequestType requestType))
            requestType = RequestType.Ignore;
        return requestType;
    }
    public void DispatchRequest(ActionRequest request)
    {
        RequestHandlerChain.GetInstance().HandleRequest(request);
    }
    // to exract the desired binding just read the key from console as usual
    // For not KeyBindings are constant, but I will provide functionality to allow user anytime during the game to change them for the desired ones
    public KeyboardTranslator()
    {
        KeyBindings.Add((ConsoleKey.W, ConsoleModifiers.None), RequestType.MoveUp);
        KeyBindings.Add((ConsoleKey.A, ConsoleModifiers.None), RequestType.MoveLeft);
        KeyBindings.Add((ConsoleKey.S, ConsoleModifiers.None), RequestType.MoveDown);
        KeyBindings.Add((ConsoleKey.D, ConsoleModifiers.None), RequestType.MoveRight);
        KeyBindings.Add((ConsoleKey.G, ConsoleModifiers.None), RequestType.DropItem);
        KeyBindings.Add((ConsoleKey.G, ConsoleModifiers.Control), RequestType.EmptyInventory);
        KeyBindings.Add((ConsoleKey.K, ConsoleModifiers.None), RequestType.HideControls);
        KeyBindings.Add((ConsoleKey.U, ConsoleModifiers.None), RequestType.UseItem);
        KeyBindings.Add((ConsoleKey.Q, ConsoleModifiers.None), RequestType.EquipItem);
        KeyBindings.Add((ConsoleKey.H, ConsoleModifiers.None), RequestType.ScopeHands);
        KeyBindings.Add((ConsoleKey.I, ConsoleModifiers.None), RequestType.ScopeInventory);
        KeyBindings.Add((ConsoleKey.E, ConsoleModifiers.None), RequestType.PickUpItem);
        KeyBindings.Add((ConsoleKey.X, ConsoleModifiers.None), RequestType.Quit);
        KeyBindings.Add((ConsoleKey.D1, ConsoleModifiers.None), RequestType.NormalAttack);
        KeyBindings.Add((ConsoleKey.D2, ConsoleModifiers.None), RequestType.StealthAttack);
        KeyBindings.Add((ConsoleKey.D3, ConsoleModifiers.None), RequestType.MagicAttack);
        KeyBindings.Add((ConsoleKey.Spacebar, ConsoleModifiers.None), RequestType.OneWeaponAttack);
        KeyBindings.Add((ConsoleKey.Spacebar, ConsoleModifiers.Shift), RequestType.TwoWeaponAttack);
        KeyBindings.Add((ConsoleKey.Escape, ConsoleModifiers.None), RequestType.ScopeRoom);
        KeyBindings.Add((ConsoleKey.LeftArrow, ConsoleModifiers.None), RequestType.PrevItem);
        KeyBindings.Add((ConsoleKey.RightArrow, ConsoleModifiers.None), RequestType.NextItem);
    }
}

using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.View;

public class ServerConsoleView : View
{
    public ServerConsoleView() : base(true)
    {
    }
    public override void ClearLogMessage()
    {
        throw new NotImplementedException();
    }

    public override void DisplayRoutine()
    {
        throw new NotImplementedException();
    }

    public override void HideControls()
    {
        throw new NotImplementedException();
    }

    public override void ResetFocusType()
    {
        throw new NotImplementedException();
    }

    public override void SetHandsFocus()
    {
        throw new NotImplementedException();
    }

    public override void SetInventoryFocus()
    {
        throw new NotImplementedException();
    }

    public override void ShiftCurrentFocus(Direction direction)
    {
        throw new NotImplementedException();
    }

    public override void WelcomeRoutine()
    {
        throw new NotImplementedException();
    }

    public override void EndRoutine(bool flag)
    {
        base.EndRoutine(flag);
        Console.WriteLine("Stopping server...");
    }
}

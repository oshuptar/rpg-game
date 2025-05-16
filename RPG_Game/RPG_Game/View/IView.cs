using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.UIHandlers;

// Will define contract on UIHandlers
public interface IView
{
    void WelcomeRoutine();
    void DisplayRoutine();
}

using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.UIHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.View;

public abstract class View : IView
{
    private RoomState? RoomState { get; set; }
    public abstract void DisplayRoutine();
    public abstract void WelcomeRoutine();
    public IInputHandler GameInputHandler { get; set; } = new KeyboardTranslator();

    //public SendRequest(RequestType type)
    public View() { }
    public View(RoomState? roomState)
    {
        RoomState = roomState;
    }
    public void SetRoomState(RoomState roomState) => RoomState = roomState;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.View;

public interface IViewCommand
{
    //public void SetView(View view);
    public void Execute();
}

public interface IServerViewCommand : IViewCommand
{ 
    public void SetView(ServerView serverView);
}

public interface IClientViewCommand : IViewCommand
{
    public void SetView(ClientView clientView);
}



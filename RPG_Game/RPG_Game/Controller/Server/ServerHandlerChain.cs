using RPG_Game.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

// We can use a Linked List data structure, however I decided to write my own
public class ServerHandlerChain
{
    public ServerController ServerController { get; private set; }
    public static ServerHandlerChain? Chain = null;
    private IServerRequestHandler? _firstHandler;
    private IServerRequestHandler? _lastHandler;
    private ServerHandlerChain()
    {
        _firstHandler = null;
        _lastHandler = null;
    }
    public static ServerHandlerChain GetInstance()
    {
        if (Chain == null)
            Chain = new ServerHandlerChain();
        return Chain;
    }
    public List<IServerViewCommand>? HandleRequest(Request request)
    {
        return _firstHandler?.HandleRequest(request);
    }

    // Fix implementation, make more concise
    public void AddHandler(IServerRequestHandler handler)
    {
        if (_firstHandler == null)
        {
            _firstHandler = handler;
            _lastHandler = handler;
            _firstHandler.SetNext(new ServerRequestHandler());
        }
        _lastHandler?.SetNext(handler);
        _lastHandler = handler;
        _lastHandler.SetNext(new ServerRequestHandler());
    }

    public void SetServerController(ServerController serverController)
    {
        ServerController = serverController;
    }
}
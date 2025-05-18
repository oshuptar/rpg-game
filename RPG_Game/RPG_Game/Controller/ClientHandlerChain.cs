using RPG_Game.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

public class ClientHandlerChain
{
    public ClientController ClientController { get; private set; }
    public static ClientHandlerChain? Chain = null;
    private IRequestHandler? _firstHandler;
    private IRequestHandler? _lastHandler;
    private ClientHandlerChain()
    {
        _firstHandler = null;
        _lastHandler = null;
    }
    public static ClientHandlerChain GetInstance()
    {
        if (Chain == null)
            Chain = new ClientHandlerChain();
        return Chain;
    }
    public List<IViewCommand>? HandleRequest(Request request)
    {
        return _firstHandler?.HandleRequest(request);
    }
    public void SetClientController(ClientController clientController)
    {
        ClientController = clientController;
    }
    public void AddHandler(IRequestHandler handler)
    {
        if (_firstHandler == null)
        {
            _firstHandler = handler;
            _lastHandler = handler;
            _firstHandler.SetNext(new ClientRequestHandler());
        }
        _lastHandler?.SetNext(handler);
        _lastHandler = handler;
        _lastHandler.SetNext(new ClientRequestHandler());
    }
}

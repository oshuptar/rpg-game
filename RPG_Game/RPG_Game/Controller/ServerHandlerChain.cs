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
    public static ServerHandlerChain? Chain = null;
    private IRequestHandler? _firstHandler;
    private IRequestHandler? _lastHandler;
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
    public void HandleRequest(ActionRequest request)
    {
        _firstHandler?.HandleRequest(request);
    }

    // Fix implementation, make more concise
    public void AddHandler(IRequestHandler handler)
    {
        if (_firstHandler == null)
        {
            _firstHandler = handler;
            _lastHandler = handler;
            _firstHandler.SetNext(new DefaultHandler());
        }
        _lastHandler?.SetNext(handler);
        _lastHandler = handler;
        _lastHandler.SetNext(new DefaultHandler());
    }
}
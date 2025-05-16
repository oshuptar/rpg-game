using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Controller;

// We can use a Linked List data structure, however I decided to write my own
public class RequestHandlerChain
{
    public static RequestHandlerChain? Chain = null;
    private IRequestHandler? _firstHandler;
    private IRequestHandler? _lastHandler;
    private RequestHandlerChain()
    {
        _firstHandler = null;
        _lastHandler = null;
    }
    public static RequestHandlerChain GetInstance()
    {
        if (Chain == null)
            Chain = new RequestHandlerChain();
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
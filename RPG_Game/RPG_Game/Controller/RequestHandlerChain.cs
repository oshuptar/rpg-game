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
    public static RequestHandlerChain? chain = null;
    private IRequestHandler? firstHandler;
    private IRequestHandler? lastHandler;

    private RequestHandlerChain()
    {
        firstHandler = null;
        lastHandler = null;
    }

    public static RequestHandlerChain GetInstance()
    {
        if (chain == null)
            chain = new RequestHandlerChain();
        return chain;
    }

    public void HandleRequest(ActionRequest request)
    {
        firstHandler?.HandleRequest(request);
    }

    public void AddHandler(IRequestHandler handler)
    {
        if (firstHandler == null)
        {
            firstHandler = handler;
            lastHandler = handler;
        }
        lastHandler?.SetNext(handler);
        lastHandler = handler;
    }
}

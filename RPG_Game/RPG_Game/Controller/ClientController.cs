using RPG_Game.Enums;
using RPG_Game.Model;
using RPG_Game.UIHandlers;
using RPG_Game.View;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace RPG_Game.Controller;

public class ClientController : Controller
{
    public TcpClient TcpClient { get; private set; }
    public BlockingCollection<IRequest> Requests = new BlockingCollection<IRequest>(new ConcurrentQueue<IRequest>());
    private ClientHandlerChain ClientChain = ClientHandlerChain.GetInstance();
    public ClientController(View.View view, Room gameState) : base(view, gameState)
    {}
    public override void SendRequest(IRequest request)
    {
        Requests.Add(request);
    }
    public override void HandleRequest(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (Requests.TryTake(out IRequest? request))
            {
                if (request != null)
                {
                    Request handledRequest = (Request)request;
                    handledRequest.GameState = GetGameState();
                    List<IViewCommand>? Commands = ClientChain.HandleRequest(handledRequest);
                    if (Commands != null)
                    {
                        foreach (IViewCommand command in Commands)
                        {
                            View.SendCommand(command);
                        }
                    }
                }
            }
        }
    }
    public void ClientRun()
    {
        Task.Run(() => HandleRequest(View.CancellationTokenSource.Token));
        Task.Run(() => View.ReadInput(View.CancellationTokenSource.Token));
        Task.Run(() => Listen(View.CancellationTokenSource.Token));
        View.HandleCommand();

        TcpClient.GetStream().Dispose();
    }
    public void Listen(CancellationToken cancellationToken)
    {
        Console.WriteLine("Listening...");
        while (!cancellationToken.IsCancellationRequested)
        {
            byte[] payloadLengthBuffer = new byte[4];
            TcpClient.GetStream().ReadExactly(payloadLengthBuffer, 0, 4);

            int payloadLength = BitConverter.ToInt32(payloadLengthBuffer, 0);
            payloadLength = IPAddress.NetworkToHostOrder(payloadLength);

            byte[] payload = new byte[payloadLength];
            TcpClient.GetStream().ReadExactly(payload, 0, payloadLength);

            string json = Encoding.UTF8.GetString(payload);

            Console.WriteLine("Deserializing");
            Response? response = JsonSerializer.Deserialize<Response>(json);
            Console.WriteLine($"Deserialized");
            if (response == null) return;

            response.Controller = this;
            response.HandleResponse();
        }
    }
    public override void HandleResponse(IResponse response)
    {
        View.SendCommand(new ResponseCommand(response.GetGameState()));
    }
    public void SendNetworkRequest(Request request)
    {
        string json = JsonSerializer.Serialize<Request>(request, new JsonSerializerOptions() 
        { ReferenceHandler = ReferenceHandler.Preserve, WriteIndented = true });
        var payload = Encoding.UTF8.GetBytes(json);
        int payloadLength = payload.Length;

        payloadLength = IPAddress.HostToNetworkOrder(payloadLength); 
        byte[] payloadLengthBuffer = BitConverter.GetBytes(payloadLength);

        TcpClient.GetStream().Write(payloadLengthBuffer, 0, payloadLengthBuffer.Length);
        TcpClient.GetStream().Write(payload, 0, payload.Length);
    }
    public void QuitGame()
    {
        // Cancelling threads and handling cleanup
        // Set State to ClientEnd
        
    }
    public void SetNetworkStream(TcpClient tcpClient)
    {
        TcpClient = tcpClient;
    }
    public void BuildChain()
    {
        ClientChain.AddHandler(new ScopeInventoryHandler());
        ClientChain.AddHandler(new ScopeRoomHandler());
        ClientChain.AddHandler(new ScopeHandsHandler());
        ClientChain.AddHandler(new HideControlsHandler());
        ClientChain.AddHandler(new NextItemHandler());
        ClientChain.AddHandler(new PrevItemHandler());
        ClientChain.AddHandler(new NormalAttackModeClientHandler());
        ClientChain.AddHandler(new StealthAttackModeClientHandler());
        ClientChain.AddHandler(new MagicAttackModeClientHandler());
        ClientChain.AddHandler(new OneWeaponAttackClientHandler());
        ClientChain.AddHandler(new TwoWeaponAttackClientHandler());
        ClientChain.AddHandler(new UseItemClientHandler());
        ClientChain.AddHandler(new MoveUpClientHandler());
        ClientChain.AddHandler(new MoveDownClientHandler());
        ClientChain.AddHandler(new MoveRightClientHandler());
        ClientChain.AddHandler(new MoveLeftClientHandler());
        ClientChain.AddHandler(new PickUpItemClientHandler());
        ClientChain.AddHandler(new DropItemClientHandler());
        ClientChain.AddHandler(new EquipItemClientHandler());
        ClientChain.AddHandler(new EmptyInventoryClientHandler());
        ClientChain.AddHandler(new QuitClientHadler());
    }
}

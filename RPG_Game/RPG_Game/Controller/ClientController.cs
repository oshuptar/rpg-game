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

public class ClientController : Controller, IClientController
{
    public GameState GameState { get; private set; }
    public ClientView ClientView { get; private set; }
    public TcpClient TcpClient { get; private set; }

    //public BlockingCollection<IRequest> Requests = new BlockingCollection<IRequest>(new ConcurrentQueue<IRequest>());

    private ClientHandlerChain ClientChain = ClientHandlerChain.GetInstance();
    public ClientController(ClientView clientView, GameState gameState) /*: base(view, gameState)*/
    {
        GameState = gameState;
        ClientView = clientView;
        //ClientChain.SetClientController(this);
    }
    public void ClientRun()
    {
        Task.Run(() => HandleRequest(ClientView.CancellationTokenSource.Token));
        Task.Run(() => ClientView.ReadInput(ClientView.CancellationTokenSource.Token));
        Task.Run(() => Listen(ClientView.CancellationTokenSource.Token));
        ClientView.HandleCommand();
        //TcpClient.GetStream().Dispose();
    }
    public override void SendRequest(IRequest request)
    {
        Requests.Add(request);
    }
    public override void HandleRequest(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            IRequest request = Requests.Take();
            if (request != null)
            {
                Request handledRequest = (Request)request;
                handledRequest.GameState = GetGameState();
                List<IClientViewCommand>? Commands = ClientChain.HandleRequest(handledRequest);
                if (Commands != null)
                {
                    foreach (IClientViewCommand command in Commands)
                    {
                        command.SetView(ClientView);
                        ClientView.SendCommand(command);
                    }
                }
            }

        }
    }
    public void Listen(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            byte[] payloadLengthBuffer = new byte[4];
            TcpClient.GetStream().ReadExactly(payloadLengthBuffer, 0, 4);

            int payloadLength = BitConverter.ToInt32(payloadLengthBuffer, 0);
            payloadLength = IPAddress.NetworkToHostOrder(payloadLength);

            byte[] payload = new byte[payloadLength];
            TcpClient.GetStream().ReadExactly(payload, 0, payloadLength);

            string json = Encoding.UTF8.GetString(payload);

            Response? response = JsonSerializer.Deserialize<Response>(json, new JsonSerializerOptions
            { ReferenceHandler = ReferenceHandler.Preserve });

            if (response == null) continue;

            response.GameState.PlayerId = response.PlayerId.Value;
            response.Controller = this;
            SetGameState(response.GameState);
            response.HandleResponse();
        }
    }
    public override void HandleResponse(IResponse response)
    {
        ResponseCommand responseCommand = new ResponseCommand(response.GetGameState());
        responseCommand.SetView(ClientView);
        ClientView.SendCommand(responseCommand);
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
    public void SetNetworkStream(TcpClient tcpClient)
    {
        TcpClient = tcpClient;
    }
    public override void SetViewController()
    {
        ClientView.SetController(this);
    }
    public void SetView(ClientView clientView)
    {
        ClientView = clientView;
    }
    public void SetGameState(GameState gameState)
    {
        GameState = gameState;
    }
    public GameState GetGameState()
    {
        return GameState.GetGameState();
    }
}

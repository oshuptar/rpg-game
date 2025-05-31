using RPG_Game.Enums;
using RPG_Game.Model;
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

    private ClientHandlerChain ClientChain = ClientHandlerChain.GetInstance();

    public BlockingCollection<Request> ScheduledRequests = new(new ConcurrentQueue<Request>());
    public ClientController(ClientView clientView, GameState gameState)
    {
        GameState = gameState;
        ClientView = clientView;
    }
    public void ClientRun()
    {
        Task.Run(() => HandleRequest(ClientView.CancellationTokenSource.Token));
        Task.Run(() => ClientView.ReadInput(ClientView.CancellationTokenSource.Token));
        Task.Run(async () => await Listen(ClientView.CancellationTokenSource.Token));
        Task.Run((async () => await SendNetworkRequest(ClientView.CancellationTokenSource.Token)));
        ClientView.HandleCommand();
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
    public async Task Listen(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                byte[] payloadLengthBuffer = new byte[4];
                await TcpClient.GetStream().ReadExactlyAsync(payloadLengthBuffer, 0, 4);

                int payloadLength = BitConverter.ToInt32(payloadLengthBuffer, 0);
                payloadLength = IPAddress.NetworkToHostOrder(payloadLength);

                byte[] payload = new byte[payloadLength];
                await TcpClient.GetStream().ReadExactlyAsync(payload, 0, payloadLength);

                string json = Encoding.UTF8.GetString(payload);

                Response? response = JsonSerializer.Deserialize<Response>(json, new JsonSerializerOptions
                { ReferenceHandler = ReferenceHandler.Preserve });

                if (response == null) continue;

                response.GameState.PlayerId = response.PlayerId.Value;
                response.Controller = this;
                SetGameState(response.GameState);
                response.HandleResponse();
            }
            catch (Exception)
            {
                IClientViewCommand viewCommand = new QuitCommand();
                viewCommand.SetView(ClientView);
                ClientView.SendCommand(viewCommand);
            }
        }
    }
    public override void HandleResponse(IResponse response)
    {
        ResponseCommand responseCommand = new ResponseCommand(response.GetGameState());
        responseCommand.SetView(ClientView);
        ClientView.SendCommand(responseCommand);
    }
    public void ScheduleRequest(Request request)
    {
        ScheduledRequests.Add(request);
    }
    public async Task SendNetworkRequest(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Request request = ScheduledRequests.Take();
            try
            {
                string json = JsonSerializer.Serialize<Request>(request, new JsonSerializerOptions()
                { ReferenceHandler = ReferenceHandler.Preserve, WriteIndented = true });
                var payload = Encoding.UTF8.GetBytes(json);
                int payloadLength = payload.Length;

                payloadLength = IPAddress.HostToNetworkOrder(payloadLength);
                byte[] payloadLengthBuffer = BitConverter.GetBytes(payloadLength);

                await TcpClient.GetStream().WriteAsync(payloadLengthBuffer, 0, payloadLengthBuffer.Length);
                await TcpClient.GetStream().WriteAsync(payload, 0, payload.Length);
            }
            catch (Exception)
            {
                IClientViewCommand viewCommand = new QuitCommand();
                viewCommand.SetView(ClientView);
                ClientView.SendCommand(viewCommand);
            }
        }
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

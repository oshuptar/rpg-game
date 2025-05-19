using RPG_Game.Enums;
using RPG_Game.Model;
using RPG_Game.UIHandlers;
using RPG_Game.View;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace RPG_Game.Controller;

public class ServerController : Controller
{
    private static int playerId = 1;
    public TcpListener Server { get; private set; }
    public ConcurrentDictionary<int, TcpClient> ActiveClients = new();
    public BlockingCollection<IRequest> Requests = new BlockingCollection<IRequest>(new ConcurrentQueue<IRequest>());
    private ServerHandlerChain ServerChain = ServerHandlerChain.GetInstance();
    public BlockingCollection<(int?, IResponse)> OutputResponses = new BlockingCollection<(int?, IResponse)>(new ConcurrentQueue<(int?, IResponse)>());
    public ServerController(View.View view, Room gameState) : base(view, gameState)
    {
    }
    public override void HandleRequest(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (Requests.TryTake(out var request))
            {
                Request handledRequest = (Request)request;
                handledRequest.GameState = Room;
                List<IViewCommand>? Commands = ServerChain.HandleRequest(handledRequest);
                if (Commands != null)
                {
                    foreach (IViewCommand command in Commands)
                    {
                        command.SetView(View);
                        View.SendCommand(command);
                    }
                }
            }
        }
    }
    public void ServerRun()
    {
        Task.Run(() => AcceptConnections(View.CancellationTokenSource.Token));
        Task.Run(() => SendResponse(View.CancellationTokenSource.Token));
        Task.Run(() => HandleRequest(View.CancellationTokenSource.Token));
        Task.Run(() => View.ReadInput(View.CancellationTokenSource.Token));
        View.HandleCommand();
    }
    public void SendResponse(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var response = OutputResponses.Take();
            List<int> clients = new List<int>();

            if (response.Item1 == null)
                clients = ActiveClients.Keys.ToList();
            else
                clients.Add(response.Item1.Value);
            foreach (var clientId in clients)
            {
                Response sentResponse = (Response)response.Item2;
                sentResponse.PlayerId = clientId;

                string json = JsonSerializer.Serialize<Response>(sentResponse, new JsonSerializerOptions
                { WriteIndented = true, ReferenceHandler = ReferenceHandler.Preserve });
                byte[] payload = Encoding.UTF8.GetBytes(json);
                int payloadLength = payload.Length;

                payloadLength = IPAddress.HostToNetworkOrder(payloadLength);
                byte[] payloadLengthBuffer = BitConverter.GetBytes(payloadLength);

                ActiveClients[clientId].GetStream().Write(payloadLengthBuffer, 0, payloadLengthBuffer.Length);
                ActiveClients[clientId].GetStream().Write(payload, 0, payload.Length);
            }
        }
    }
    public override void HandleResponse(IResponse response)
    {
        OutputResponses.Add((response.GetPlayerId(), response));
    }

    public override void SendRequest(IRequest request)
    {
        Requests.Add(request);
    }
    public void AcceptConnections(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("Waiting for connection...");
            TcpClient client = Server.AcceptTcpClient();
            Console.WriteLine($"Client {playerId} connected");
            ActiveClients.TryAdd(playerId, client);

            //playerId is captured by reference in the lambda — and its value changes before the lambda executes.
            Room.AddPlayer(new Player(playerId), new Position(2 * (playerId - 1) + 1, 1));
            int clientId = playerId;
            Task.Run(() => Listen(clientId, View.CancellationTokenSource.Token));
            playerId++;

            if (playerId > 9) return;
        }
    }

    public void Listen(int playerID, CancellationToken cancellationToken)
    {
        Response response = new Response(RequestType.ClientJoined,
                null,
                GetGameState());
        response.Controller = this;
        response.HandleResponse();

        ActiveClients.TryGetValue(playerID, out var client);
        if (client == null) return;
        while (!cancellationToken.IsCancellationRequested)
        {
            byte[] payloadLengthBuffer = new byte[4];
            client.GetStream().ReadExactly(payloadLengthBuffer, 0, 4);

            int payloadLength = BitConverter.ToInt32(payloadLengthBuffer, 0);
            payloadLength = IPAddress.NetworkToHostOrder(payloadLength);

            byte[] payload = new byte[payloadLength];
            client.GetStream().ReadExactly(payload, 0, payloadLength);

            string json = Encoding.UTF8.GetString(payload);
            Request? request = JsonSerializer.Deserialize<Request>(json,
                new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, WriteIndented = true });
            //if (request == null) return;

            request.Receiver = this;
            request.GameState = Room;
            request.GameState.PlayerId = playerID;
            request.PlayerId = playerID;
            request.SendRequest();
        }
    }

    public void SendNetworkRequest()
    {

    }
    public void SetTcpListener(TcpListener tcpListener)
    {
        Server = tcpListener;
    }
}

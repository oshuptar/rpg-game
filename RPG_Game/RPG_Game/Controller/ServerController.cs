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

// Add exception handling
public class ServerController : Controller, IServerController
{
    private static int playerId = 1;
    public AuthorityGameState GameState { get; private set; }
    public ServerView ServerView { get; private set; }
    public TcpListener Server { get; private set; }

    public ConcurrentDictionary<int, TcpClient> ActiveClients = new();

    //public BlockingCollection<IRequest> Requests = new BlockingCollection<IRequest>(new ConcurrentQueue<IRequest>());

    private ServerHandlerChain ServerChain = ServerHandlerChain.GetInstance();

    public BlockingCollection<(int?, IResponse)> OutputResponses = new BlockingCollection<(int?, IResponse)>(new ConcurrentQueue<(int?, IResponse)>());
    public ServerController(ServerView serverView, AuthorityGameState gameState) /*: base(view, gameState)*/
    {
        GameState = gameState;
        ServerView = serverView;
    }
    public void ServerRun()
    {
        Task.Run(() => AcceptConnections(ServerView.CancellationTokenSource.Token));
        Task.Run(() => SendResponse(ServerView.CancellationTokenSource.Token));
        Task.Run(() => HandleRequest(ServerView.CancellationTokenSource.Token));
        Task.Run(() => ServerView.ReadInput(ServerView.CancellationTokenSource.Token));
        ServerView.HandleCommand();
    }
    public override void SendRequest(IRequest request)
    {
        Requests.Add(request);
    }
    public override void HandleRequest(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        { 
            // Extract this method to the abstract class
            var request = Requests.Take();
            Request handledRequest = (Request)request;
            //handledRequest.GameState = GetGameState();
            List<IServerViewCommand>? Commands = ServerChain.HandleRequest(handledRequest);
            if (Commands != null)
            {
                foreach (IServerViewCommand command in Commands)
                {
                    command.SetView(ServerView);
                    ServerView.SendCommand(command);
                }
            }
        }
    }
    public void Listen(CancellationToken cancellationToken, int playerID)
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

            if (request == null) continue;

            request.Receiver = this;
            request.GameState = GameState;
            request.GameState.PlayerId = playerID;
            //request.PlayerId = playerID;
            request.SendRequest();
        }
    }
    public override void HandleResponse(IResponse response)
    {
        OutputResponses.Add((response.GetPlayerId(), response));
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
    public void AcceptConnections(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            //Console.WriteLine("Waiting for connection...");
            TcpClient client = Server.AcceptTcpClient();
            //Console.WriteLine($"Client {playerId} connected");
            ActiveClients.TryAdd(playerId, client);

            //playerId is captured by reference in the lambda — and its value changes before the lambda executes.
            GameState.AddPlayer(new Player(playerId), new Position(2 * (playerId - 1) + 1, 1));
            int clientId = playerId;
            Task.Run(() => Listen(ServerView.CancellationTokenSource.Token, clientId));
            playerId++;

            if (playerId > 9) return;
        }
    }

    public void SetTcpListener(TcpListener tcpListener)
    {
        Server = tcpListener;
    }

    public override void SetViewController()
    {
        ServerView.SetController(this);
    }
    public void SetView(ServerView serverView)
    {
        ServerView = serverView;
    }

    public void SetGameState(AuthorityGameState gameState)
    {
        GameState = gameState;
    }
    public GameState GetGameState()
    {
        return GameState.GetGameState();
    }
}

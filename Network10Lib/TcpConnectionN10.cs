using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Network10Lib
{
    public class TcpConnectionN10
    {
        /// <summary>
        /// Returns true if this connection is a server
        /// </summary>
        public bool IsServer { get; private set; }
        /// <summary>
        /// returns true after a server or client was opened und stay true until connection is lost or Close() was called
        /// </summary>
        public bool IsConnected => (connector != null && myAdr != -1);
        /// <summary>
        /// A client will send this string driectly after tcp connection is opended. The server will only accept clients whcih send this string first.
        /// Note: This is just to verify compatibility. This does not provide any security.
        /// </summary>
        public string ClientConnectionId { private get; init; } = "DefaultConnectionIdClientV1.0.0";
        /// <summary>
        /// If a the client provides a valid ClientConnectionId, the server will anser with the ServerConnectionId. The client will only connect to a server with a valid ServerConnectionId.
        /// </summary>
        public string ServerConnectionId { private get; init; } = "DefaultConnectionIdServerV1.0.0";

        /// <summary>
        /// A new player connected to the server. This event is only raised on the server.
        /// </summary>
        /// <param name="playerNr">0: server, 1,2,... newly connected client</param>
        public delegate void PlayerConnectedHandler(int playerNr);
        /// <summary>
        /// A new player connected to the server. This event is only raised on the server.
        /// </summary>
        public event PlayerConnectedHandler? PlayerConnected;
        /// <summary>
        /// A player disconnected from the server. This event is only raised on the server.
        /// </summary>
        /// <param name="playerNr">0: server, 1,2,... newly connected client</param>
        public delegate void PlayerDisconnectedHandler(int playerNr);
        /// <summary>
        /// A player disconnected from the server. This event is only raised on the server.
        /// </summary>
        public event PlayerDisconnectedHandler? PlayerDisonnected;

        /// <summary>
        /// A message was received from the tcp connection.
        /// </summary>
        /// <param name="msg"></param>
        public delegate void MessageReceivedHandler(MessageN10 msg);
        /// <summary>
        /// A message was received from the tcp connection.
        /// </summary>
        public event MessageReceivedHandler? MessageReceived;

        /// <summary>
        /// This event is raised during a successful OpenServer() or OpenClient() call
        /// </summary>
        public delegate void ConnectedHandler();
        /// <summary>
        ///  This event is raised during a successful OpenServer() or OpenClient() call
        /// </summary>
        public event ConnectedHandler? Connected;
        /// <summary>
        /// This event is raised during a Close() call or of the connection was lost.
        /// </summary>
        public delegate void DisconnectedHandler();
        /// <summary>
        /// This event is raised during a Close() call or of the connection was lost.
        /// </summary>
        public event DisconnectedHandler? Disonnected;

        //inertnal events
        private delegate void ServerHandshakeResponseHandler(bool success);
        private event Action<bool>? ServerHandshakeResponsed;
        //internal variables
        private ITcpConnectorN10? connector = null;
        private int myAdr = -1;

        /// <summary>
        /// Creates a TcpConnectionN10 object
        /// </summary>
        public TcpConnectionN10()
        {

        }

        /// <summary>
        /// Opens a Server with default Port 12345
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task OpenServer()
        {
            if (connector is not null)
                throw new InvalidOperationException();

            TcpServerN10 server = new TcpServerN10();
            server.MessageReceived += ServerReceivedMessage;
            server.ClientDisconnected += ServerLostClient;
            connector = server;
            await connector.Connect().ConfigureAwait(false);
            myAdr = 0;
            IsServer = true;
            Connected?.Invoke();
            PlayerConnected?.Invoke(0);
        }

        /// <summary>
        /// Opens a Server on a specific Port
        /// </summary>
        /// <param name="Port">Port on which the serber is listening to new client connections</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task OpenServer(int Port)
        {
            if (connector is not null)
                throw new InvalidOperationException();

            TcpServerN10 server = new TcpServerN10 { Port = Port };
            server.MessageReceived += ServerReceivedMessage;
            server.ClientDisconnected += ServerLostClient;
            connector = server;
            await connector.Connect().ConfigureAwait(false);
            myAdr = 0;
            IsServer = true;
            Connected?.Invoke();
            PlayerConnected?.Invoke(0);
        }


        /// <summary>
        /// Opens a Server on a specific Port and NetworkCard
        /// </summary>
        /// <param name="Port">Port on which the server is listening to new client connections</param>
        /// <param name="IpAddr">Use this to limit incomming client connections to a specific network card</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task OpenServer(int Port, IPAddress IpAddr)
        {
            if (connector is not null)
                throw new InvalidOperationException();

            TcpServerN10 server = new TcpServerN10 { IPAddr = IpAddr, Port = Port };
            server.MessageReceived += ServerReceivedMessage;
            server.ClientDisconnected += ServerLostClient;
            connector = server;
            await connector.Connect().ConfigureAwait(false);
            myAdr = 0;
            IsServer = true;
            Connected?.Invoke();
            PlayerConnected?.Invoke(0);
        }

        /// <summary>
        /// Connect to a Server as a client on the default Port 12345 on this computer.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task OpenClient()
        {
            if (connector is not null)
                throw new InvalidOperationException();

            myAdr = -1;
            IsServer = false;
            TcpClientN10 client  = new TcpClientN10();
            client.MessageReceived += ClientReceivedMessage;
            client.Disconnected += ClientDisconnected;
            connector = client;
            await connector.Connect().ConfigureAwait(false);
            await SendClientHandshake().ConfigureAwait(false);
        }

        /// <summary>
        /// Connect to a Server as a client on a specific Port on this computer.
        /// </summary>
        /// <param name="Port">Port on which the server wait for new connections</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task OpenClient(int Port)
        {
            if (connector is not null)
                throw new InvalidOperationException();

            myAdr = -1;
            IsServer = false;
            TcpClientN10 client = new TcpClientN10 { Port = Port };
            client.MessageReceived += ClientReceivedMessage;
            client.Disconnected += ClientDisconnected;
            connector = client;
            await connector.Connect().ConfigureAwait(false);
            await SendClientHandshake().ConfigureAwait(false);
        }

        /// <summary>
        /// Connect to a Server as a client on a specific Port on a remote computer.
        /// </summary>
        /// <param name="Port">Port on which the server wait for new connections</param>
        /// <param name="IpAddr">address of remote computer</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task OpenClient(int Port, IPAddress IpAddr)
        {
            if (connector is not null)
                throw new InvalidOperationException();

            myAdr = -1;
            IsServer = false;
            TcpClientN10 client = new TcpClientN10 { IPAddr = IpAddr, Port = Port };
            client.MessageReceived += ClientReceivedMessage;
            client.Disconnected += ClientDisconnected;
            connector = client;
            await connector.Connect().ConfigureAwait(false);
            await SendClientHandshake().ConfigureAwait(false);
        }

        /// <summary>
        /// Close an open connection
        /// </summary>
        /// <returns></returns>
        public async Task Close()
        {
            if (connector is not null)
            {
                await connector.Disconnect();
                TcpServerN10? server;
                if ((server = connector as TcpServerN10) is not null)
                {
                    ServerDisconnected(server);
                }
                connector = null;
            }
        }

        //this connection is a client and has lost its server
        private void ClientDisconnected(TcpClientN10 sender)
        {
            sender.MessageReceived -= ClientReceivedMessage;
            sender.Disconnected -= ClientDisconnected;
            myAdr = -1;
            this.connector = null;
            Disonnected?.Invoke();
        }

        //this connection is a Server and was closed
        private void ServerDisconnected(TcpServerN10 sender)
        {
            PlayerDisonnected?.Invoke(0); //Player 0 is the server itself
            sender.MessageReceived -= ServerReceivedMessage;
            sender.ClientDisconnected -= ServerLostClient;
            myAdr = -1;
            this.connector = null;
            Disonnected?.Invoke();
        }

        //Server lost a client connection 
        private void ServerLostClient(TcpServerN10 sender, int clientNr)
        {
            PlayerDisonnected?.Invoke(clientNr + 1); //player number is clientNr + 1
        }

        //client sends a Handshake with the ClientConnectionId aund wait for an handshake answer from server
        private async Task SendClientHandshake()
        {
            AutoResetEvent arw = new AutoResetEvent(false);
            bool handshakeSuccess = false;
            Action<bool> eventCatcher = (resp) => { handshakeSuccess = resp; arw.Set(); };

            ServerHandshakeResponsed += eventCatcher;


            MessageN10 msg = new MessageN10 { Sender = -1, Receiver = 0, MsgType = MessageN10.EnumMsgType.ClientHandshake, Data = ClientConnectionId };
            await (connector?.SendMessage(msg).ConfigureAwait(false) ?? Task.CompletedTask.ConfigureAwait(false));
            

            arw.WaitOne(1000);
            ServerHandshakeResponsed -= eventCatcher;
            if (!handshakeSuccess)
            {
                await Close();
                throw new Exception("Server is not compatible");
            }
        }

        /// <summary>
        /// Sends a message with obj as its content to one player.
        /// </summary>
        /// <param name="obj">An object which will be converted to a json string and send over the tcp connection</param>
        /// <param name="playerNr">Player number which this message wil be sent to</param>
        /// <returns></returns>
        public async Task SendObject(object obj,int playerNr)
        {
            if(IsConnected && connector is not null)
            {
                await connector.SendMessage(new MessageN10 { Sender = myAdr, Receiver = playerNr, MsgType = MessageN10.EnumMsgType.Tcp, Data = obj }).ConfigureAwait(false);
            }
        }

        /// <summary>
        ///  Sends a message with obj as its content to multiple players.
        /// </summary>
        /// <param name="obj">An object which will be converted to a json string and send over the tcp connection</param>
        /// <param name="playerNrs">list of players</param>
        /// <returns></returns>
        public async Task SendObject(object obj, IEnumerable<int> playerNrs)
        {
            if (IsConnected)
            {
                List<Task> sendTasks = new List<Task>();
                MessageN10 msg = new MessageN10 { Sender = myAdr, Receiver = -1, MsgType = MessageN10.EnumMsgType.Tcp, Data = obj };

                foreach (var playerNr in playerNrs)
                {
                    sendTasks.Add(connector?.SendMessage(msg.Clone4otherReceiver(playerNr)) ?? Task.CompletedTask);
                }
                await Task.WhenAll(sendTasks).ConfigureAwait(false);
            }
        }

        //server received a message
        private void ServerReceivedMessage(TcpServerN10 sender, MessageN10 msg, int clientNr)
        {
            switch (msg.MsgType)
            {
                case MessageN10.EnumMsgType.ClientHandshake: //answer a client hanshake with a server handshake
                    string? s = msg.DeserializeData<string>();
                    if (s is not null && s == ClientConnectionId)
                    {
                        msg.Sender = 0;
                        msg.Receiver = clientNr + 1;
                        msg.MsgType = MessageN10.EnumMsgType.ServerHandshake;
                        msg.Data = ServerConnectionId;
                        connector?.SendMessage(msg).WaitE(); //important to wait here for thread safety
                        PlayerConnected?.Invoke(clientNr + 1);
                    }
                    break;
                case MessageN10.EnumMsgType.Tcp:
                    if (msg.Sender == clientNr + 1) //make sure that clients sends message with correct senderNr
                    {
                        if (msg.Receiver == 0) //this message is for the server, raise event
                        {
                            MessageReceived?.Invoke(msg);
                        }
                        else //this message is for a different client
                        {
                            connector?.SendMessage(msg).WaitE(); //important to wait here for thread safety
                        }
                    }
                    break;
            }
        }

        //client received a message
        private void ClientReceivedMessage(TcpClientN10 sender, MessageN10 msg)
        {
            switch (msg.MsgType)
            {
                case MessageN10.EnumMsgType.ServerHandshake: //the server sent a handshake back
                    string? s = msg.DeserializeData<string>();
                    if (s is not null && s == ServerConnectionId) //the hanshake answer is correct
                    {
                        myAdr = msg.Receiver;
                        Connected?.Invoke();
                        ServerHandshakeResponsed?.Invoke(true);
                    }
                    else //the hanshake answer is wrong
                    {
                        ServerHandshakeResponsed?.Invoke(false);
                    }

                    break;
                case MessageN10.EnumMsgType.Tcp: //the server send a message
                    MessageReceived?.Invoke(msg);
                    break;
            }
        }

    }
}

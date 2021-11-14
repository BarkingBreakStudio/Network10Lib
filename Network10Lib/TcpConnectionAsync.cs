using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Network10Lib
{
    public class TcpConnectionAsync
    {

        ITcpConnectorAsync? connector = null;
        private int myAdr = -1;

        public bool IsServer { get; private set; }
        public bool IsConnected => (connector != null && myAdr != -1);
        public string ClientConnectionId { get; init; } = "DefaultConnectionIdClientV0.0.1";
        public string ServerConnectionId { get; init; } = "DefaultConnectionIdServerV0.0.1";

        public delegate void MessageReceivedHandler(Message msg);
        public event MessageReceivedHandler? MessageReceived;

        public delegate void ConnectedHandler();
        public event ConnectedHandler? Connected;
        public delegate void DisconnectedHandler();
        public event DisconnectedHandler? Disonnected;

        public delegate void PlayerConnectedHandler(int playerNr);
        public event PlayerConnectedHandler? PlayerConnected;
        public delegate void PlayerDisconnectedHandler(int playerNr);
        public event PlayerDisconnectedHandler? PlayerDisonnected;

        public TcpConnectionAsync()
        {

        }

        public async Task OpenServer()
        {
            if (connector is not null)
                throw new InvalidOperationException();

            TcpServerAsync server = new TcpServerAsync();
            server.Message2Received += ServerReceivedMessage;
            connector = server;
            await connector.Connect().ConfigureAwait(false);
            myAdr = 0;
            IsServer = true;
            Connected?.Invoke();
            PlayerConnected?.Invoke(0);
        }

        public async Task OpenServer(int Port)
        {
            if (connector is not null)
                throw new InvalidOperationException();

            TcpServerAsync server = new TcpServerAsync { Port = Port };
            server.Message2Received += ServerReceivedMessage;
            connector = server;
            await connector.Connect().ConfigureAwait(false);
            myAdr = 0;
            IsServer = true;
            Connected?.Invoke();
            PlayerConnected?.Invoke(0);
        }

        public async Task OpenServer(IPAddress IpAddr, int Port)
        {
            if (connector is not null)
                throw new InvalidOperationException();

            TcpServerAsync server = new TcpServerAsync { IPAddr = IpAddr, Port = Port };
            server.Message2Received += ServerReceivedMessage;
            connector = server;
            await connector.Connect().ConfigureAwait(false);
            myAdr = 0;
            IsServer = true;
            Connected?.Invoke();
            PlayerConnected?.Invoke(0);
        }


        public async Task OpenClient()
        {
            if (connector is not null)
                throw new InvalidOperationException();

            myAdr = -1;
            IsServer = false;
            TcpClientAsync client  = new TcpClientAsync();
            client.Message2Received += ClientReceivedMessage;
            connector = client;
            await connector.Connect().ConfigureAwait(false);
            await SendClientHandshake().ConfigureAwait(false);
        }

        public async Task OpenClient(int Port)
        {
            if (connector is not null)
                throw new InvalidOperationException();

            myAdr = -1;
            IsServer = false;
            connector = new TcpClientAsync { Port = Port };
            await connector.Connect().ConfigureAwait(false);
            await SendClientHandshake().ConfigureAwait(false);
        }

        public async Task OpenClient(IPAddress IpAddr, int Port)
        {
            if (connector is not null)
                throw new InvalidOperationException();

            myAdr = -1;
            IsServer = false;
            connector = new TcpClientAsync { IPAddr = IpAddr, Port = Port };
            await connector.Connect().ConfigureAwait(false);
            await SendClientHandshake().ConfigureAwait(false);
        }

        public async Task Close()
        {
            if (connector is not null)
            {
                await connector.Disconnect();
                connector = null;
            }
        }

        private async Task SendClientHandshake()
        {
            if (connector is not null)
            {
                Message msg = new Message { Sender = -1, Receiver = 0, MsgType = Message.EnumMsgType.ClientHandshake, Data = ClientConnectionId };
                await connector.SendMessage(msg).ConfigureAwait(false);
            }
        }

        public async Task SendObject(object obj,int destination)
        {
            if(IsConnected && connector is not null)
            {
                await connector.SendMessage(new Message { Sender = myAdr, Receiver = destination, MsgType = Message.EnumMsgType.Tcp, Data = obj }).ConfigureAwait(false);
            }
        }


        private void ServerReceivedMessage(Message msg, int clientNr)
        {
            switch (msg.MsgType)
            {
                case Message.EnumMsgType.ClientHandshake:
                    string? s = msg.DeserializeData<string>();
                    if (s is not null && s == ClientConnectionId)
                    {
                        msg.Sender = 0;
                        msg.Receiver = clientNr + 1;
                        msg.MsgType = Message.EnumMsgType.ServerHandshake;
                        msg.Data = ServerConnectionId;
                        connector?.SendMessage(msg).Wait(); //important to wait here for thread safety
                        PlayerConnected?.Invoke(clientNr + 1); //todo: there could potentailly be a client which tries to send multiple ClientHandshakes
                    }
                    break;
                case Message.EnumMsgType.Tcp:
                    if (msg.Receiver == 0)
                    {
                        MessageReceived?.Invoke(msg);
                    }
                    else
                    {
                        connector?.SendMessage(msg).Wait(); //important to wait here for thread safety
                    }
                    break;
            }
        }

        private void ClientReceivedMessage(Message msg)
        {
            switch (msg.MsgType)
            {
                case Message.EnumMsgType.ServerHandshake:
                    string? s = msg.DeserializeData<string>();
                    if (s is not null && s == ServerConnectionId)
                    {
                        myAdr = msg.Receiver;
                        Connected?.Invoke();
                    }
                    //todo else close connection
                    break;
                case Message.EnumMsgType.Tcp:
                    MessageReceived?.Invoke(msg);
                    break;
            }
        }



        public class Message
        {
            public enum EnumMsgType
            {
                Tcp,
                Health,
                ClientHandshake,
                ServerHandshake,
            }

            public int Sender { get; set; }
            public int Receiver { get; set; }
            public EnumMsgType MsgType { get; set; }
            public object Data { get; set; } = "";

            public string Serialize()
            {
                return JsonSerializer.Serialize(this, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }

            public static Message? Deserialize(string s)
            {
                return JsonSerializer.Deserialize<Message>(s, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }

            public T? DeserializeData<T>()
            {
                if (Data is not null)
                {
                    return JsonSerializer.Deserialize<T>((JsonElement)Data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                }
                else
                {
                    return default(T);
                }
            }

        }


    }
}

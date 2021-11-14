using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network10Lib;
public class TcpServerAsync : ITcpConnectorAsync
{

    public delegate void MessageReceivedHandler(TcpServerAsync sender, int clientNr, TcpClient client, string message);
    public event MessageReceivedHandler? MessageReceived;
    public delegate void MessageReceivedHandler2(TcpConnectionAsync.Message msg, int clientNr);
    public event MessageReceivedHandler2? Message2Received;
    public delegate void ClientConnectedHandler(TcpServerAsync sender, int clientNr, TcpClient client);
    public event ClientConnectedHandler? ClientConnected;
    public delegate void ClientDisconnectedHandler(TcpServerAsync sender, int clientNr, TcpClient client);
    public event ClientDisconnectedHandler? ClientDisconnected;

    TcpListener? tcpListener = null;

    CancellationTokenSource cts = new CancellationTokenSource();
    Task? tListen;
    List<Task> clientTasks = new List<Task>();
    List<TcpClient> clients = new List<TcpClient>();
    private static UTF8Encoding encoding = new UTF8Encoding();

    public IPAddress IPAddr { get; init;} = IPAddress.Any;
    public int Port { get; init; } = 12345;



    public TcpServerAsync()
    {
    }

    public TcpServerAsync(IPAddress IPAddr, int Port)
    {
        this.IPAddr = IPAddr;
        this.Port = Port;
    }

    public Task Connect()
    {
        if (tcpListener is null)
        {
            tcpListener = new TcpListener(IPAddr, 12345);
            tcpListener.Start();
            tListen = Task.Factory.StartNew(() => { AcceptClientAsync(tcpListener).Wait(); }, TaskCreationOptions.LongRunning);
        }
        return Task.CompletedTask;
    }


    private async Task AcceptClientAsync(TcpListener tcpListener)
    {
        try
        {
            while (true)
            {
                TcpClient client = await tcpListener.AcceptTcpClientAsync(cts.Token).ConfigureAwait(false);
                int clientNr = clientTasks.Count;
                ClientConnected?.Invoke(this, clientNr, client);
                clients.Add(client);
                clientTasks.Add(Task.Factory.StartNew(() => { ReadClientData(client, clientNr).Wait(); }, TaskCreationOptions.LongRunning));
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            tcpListener.Stop();
        }
    }

    private async Task ReadClientData(TcpClient client, int clientNr)
    {
        byte[] buffer = new byte[1024];
        try
        {
            while (true)
            {
                await client.GetStream().ReadUntilLengthAsync(buffer, 4, cts.Token).ConfigureAwait(false); //throws OperationCanceledException
                int dataLength = BitConverter.ToInt32(buffer);
                if (buffer.Length < dataLength)
                {
                    buffer = new byte[dataLength];
                }
                await client.GetStream().ReadUntilLengthAsync(buffer, dataLength, cts.Token).ConfigureAwait(false); //throws OperationCanceledException
                string recvString = encoding.GetString(buffer, 0, dataLength);
                MessageReceived?.Invoke(this, clientNr, client, recvString);
                TcpConnectionAsync.Message? msg = TcpConnectionAsync.Message.Deserialize(recvString);
                if (msg is not null)
                {
                    Message2Received?.Invoke(msg, clientNr);
                }
            }
        }
        catch (OperationCanceledException){}
        finally
        {
            ClientDisconnected?.Invoke(this, clientNr, client);
            client.Close();
            client.Dispose();
        }
    }

    public async Task Write(int clientNr,string text)
    {
        if (clientNr < clients.Count)
        {
            byte[] buffer = encoding.GetBytes(text);
            await clients[clientNr].GetStream().WriteAsync(buffer, 0, buffer.Length);
        }
    }

    public async Task Write(int clientNr, byte[] buffer)
    {
        if (clientNr < clients.Count)
        {
            await clients[clientNr].GetStream().WriteAsync(buffer);
        }
    }

    public async Task Write(TcpClient client, string text)
    {
        byte[] buffer = encoding.GetBytes(text);
        await client.GetStream().WriteAsync(buffer, 0, buffer.Length);
    }

    public async Task Write(TcpClient client, byte[] buffer)
    {
        await client.GetStream().WriteAsync(buffer);
    }

    public async Task SendMessage(TcpConnectionAsync.Message msg)
    {
        if(msg.Receiver == 0)
        {
            string json = msg.Serialize();
            var msgDes = TcpConnectionAsync.Message.Deserialize(json);
            if (msgDes is not null)
            {
                Message2Received?.Invoke(msgDes, -1);
            }
        }
        else if (msg.Receiver <= clients.Count)
        {
            TcpClient client = clients[msg.Receiver - 1];
            byte[] buffer = encoding.GetBytes(msg.Serialize());
            await client.GetStream().WriteAsync(BitConverter.GetBytes(buffer.Length)).ConfigureAwait(false);
            await client.GetStream().WriteAsync(buffer).ConfigureAwait(false);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }


    public async Task Stop()
    {
        cts.Cancel();
        if (tListen is not null)
        {
            await tListen;
        }
        await Task.WhenAll(clientTasks);

        cts.Dispose();
        tcpListener = null;
    }

    public async Task Disconnect()
    {
        await Stop().ConfigureAwait(false);
    }

}


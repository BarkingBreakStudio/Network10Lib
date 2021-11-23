using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network10Lib;
public class TcpServerN10 : ITcpConnectorN10
{
    /// <summary>
    /// Use this event if client uses SendMessage methode
    /// </summary>
    /// <param name="sender">this TcpServerN10</param>
    /// <param name="msg">Received message</param>
    /// <param name="clientNr">client id which send the message</param>
    public delegate void MessageReceivedHandler(TcpServerN10 sender, MessageN10 msg, int clientNr);
    /// <summary>
    /// Use this event if client uses SendMessage methode
    /// </summary>
    public event MessageReceivedHandler? MessageReceived;

    /// <summary>
    /// Use this event if client uses WriteString methode
    /// </summary>
    /// <param name="sender">this TcpServerN10</param>
    /// <param name="clientNr">client id which sent the string</param>
    /// <param name="message">string send by client</param>
    public delegate void StringReceivedHandler(TcpServerN10 sender, int clientNr, string message);
    /// <summary>
    /// Use this event if client uses WriteString methode
    /// </summary>
    public event StringReceivedHandler? StringReceived;

    /// <summary>
    /// Use this event if client uses WriteBytes methode
    /// </summary>
    /// <param name="sender">this TcpServerN10</param>
    /// <param name="clientNr">client id which sent the bytes</param>
    /// <param name="buffer">bytes send by client</param>
    /// <param name="bufferLength">only evaluate buffer from index 0 to bufferLength - 1</param>
    public delegate void BytesReceivedHandler(TcpServerN10 sender, int clientNr, byte[] buffer, int bufferLength);
    /// <summary>
    /// Use this event if client uses WriteBytes methode
    /// </summary>
    public event BytesReceivedHandler? BytesReceived;

    /// <summary>
    /// A new client has connected
    /// </summary>
    /// <param name="sender">this TcpServerN10</param>
    /// <param name="clientNr">client id of newly connected client</param>
    public delegate void ClientConnectedHandler(TcpServerN10 sender, int clientNr);
    /// <summary>
    /// A new client has connected
    /// </summary>
    public event ClientConnectedHandler? ClientConnected;

    /// <summary>
    /// A client has disconnected
    /// </summary>
    /// <param name="sender">this TcpServerN10</param>
    /// <param name="clientNr">client id of disconnected</param>
    public delegate void ClientDisconnectedHandler(TcpServerN10 sender, int clientNr);
    /// <summary>
    /// A client has disconnected
    /// </summary>
    public event ClientDisconnectedHandler? ClientDisconnected;

    public IPAddress IPAddr { get; init; } = IPAddress.Any;
    public int Port { get; init; } = 12345;

    TcpListener? tcpListener = null;

    CancellationTokenSource cts = new CancellationTokenSource();
    Task? tListen;
    List<Task> clientTasks = new List<Task>();
    List<TcpClient> clients = new List<TcpClient>();
    private static UTF8Encoding encoding = new UTF8Encoding();



    public TcpServerN10()
    {
    }

    public TcpServerN10(IPAddress IPAddr, int Port)
    {
        this.IPAddr = IPAddr;
        this.Port = Port;
    }

    /// <summary>
    /// Starts server which waits for clients to connect
    /// </summary>
    /// <returns>awitable Task</returns>
    public Task Connect()
    {
        if (tcpListener is null)
        {
            tcpListener = new TcpListener(IPAddr, 12345);
            tcpListener.Start();
            tListen = TaskLongRunning.Run(() => AcceptClientAsync(tcpListener).WaitE());
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
                ClientConnected?.Invoke(this, clientNr);
                clients.Add(client);
                clientTasks.Add(TaskLongRunning.Run(() => ReadClientData(client, clientNr).WaitE()));
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
                BytesReceived?.Invoke(this, clientNr, buffer, dataLength);
                StringReceived?.Invoke(this, clientNr, recvString);
                MessageN10? msg = MessageN10.TryDeserialize(recvString);
                if (msg is not null)
                {
                    MessageReceived?.Invoke(this, msg, clientNr);
                }
            }
        }
        catch (OperationCanceledException){}
        finally
        {
            ClientDisconnected?.Invoke(this, clientNr);
            client.Close();
            client.Dispose();
        }
    }

    /// <summary>
    /// Writes a string to a client
    /// </summary>
    /// <param name="clientNr">client id</param>
    /// <param name="text">send string to client</param>
    /// <returns></returns>
    public async Task WriteString(int clientNr,string text)
    {
        if (clientNr < clients.Count)
        {
            byte[] buffer = encoding.GetBytes(text);
            await clients[clientNr].GetStream().WriteAsync(BitConverter.GetBytes(buffer.Length)).ConfigureAwait(false);
            await clients[clientNr].GetStream().WriteAsync(buffer).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Writes a bytes to a client
    /// </summary>
    /// <param name="clientNr">client id</param>
    /// <param name="buffer">send bytes to client</param>
    /// <returns></returns>
    public async Task WriteBytes(int clientNr, byte[] buffer)
    {
        if (clientNr < clients.Count)
        {
            await clients[clientNr].GetStream().WriteAsync(BitConverter.GetBytes(buffer.Length)).ConfigureAwait(false);
            await clients[clientNr].GetStream().WriteAsync(buffer).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Writes a message to a client
    /// </summary>
    /// <param name="msg">message will be sent to msg.Receiver</param>
    /// <returns>awaitable task</returns>
    /// <exception cref="InvalidOperationException">msg.Receiver invalid</exception>
    public async Task SendMessage(MessageN10 msg)
    {
        if(msg.Receiver == 0)
        {
            string jsonSer = msg.Serialize();
            var msgDes = MessageN10.TryDeserialize(jsonSer);
            if (msgDes is not null)
            {
                MessageReceived?.Invoke(this, msgDes, -1);
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

    /// <summary>
    /// Disconnects all clients first, then stops server
    /// </summary>
    /// <returns>awaitable Task</returns>
    public async Task Disconnect()
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

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network10Lib;

/// <summary>
/// A client is used to connect to a server
/// </summary>
public class TcpClientN10 : ITcpConnectorN10
{
    /// <summary>
    /// Use this event if server uses SendMessage methode
    /// </summary>
    /// <param name="sender">this TcpClientN10</param>
    /// <param name="msg">Received message</param>
    public delegate void MessageReceivedHandler(TcpClientN10 sender, MessageN10 msg);
    /// <summary>
    /// Use this event if server uses SendMessage method
    /// </summary>
    public event MessageReceivedHandler? MessageReceived;

    /// <summary>
    /// Use this event if server uses WriteString method
    /// </summary>
    /// <param name="sender">this TcpClientN10</param>
    /// <param name="message">received string</param>
    public delegate void StringReceivedHandler(TcpClientN10 sender, string message);
    /// <summary>
    /// Use this event if server uses WriteString method
    /// </summary>
    public event StringReceivedHandler? StringReceived;

    /// <summary>
    /// Use this event if server uses WriteBytes method
    /// </summary>
    /// <param name="sender">this TcpClientN10</param>
    /// <param name="buffer">received raw buffer</param>
    /// <param name="bufferLength">only read buffer from 0 to bufferLength - 1</param>
    public delegate void BytesReceivedHandler(TcpClientN10 sender, byte[] buffer, int bufferLength);
    /// <summary>
    /// Use this event if server uses WriteBytes method
    /// </summary>
    public event BytesReceivedHandler? BytesReceived;

    /// <summary>
    /// Connection to server was stopped or lost
    /// </summary>
    /// <param name="sender">this TcpClientN10</param>
    public delegate void DisconnectedHandler(TcpClientN10 sender);
    /// <summary>
    /// Connection to server was stopped or lost
    /// </summary>
    public event DisconnectedHandler? Disconnected;

    public IPAddress IPAddr { get; init; } = IPAddress.Loopback;
    public int Port { get; init; } = 12345;

    TcpClient? client;
    CancellationTokenSource cts = new CancellationTokenSource();
    Task? tRead;
    SemaphoreSlim writeSema = new SemaphoreSlim(1, 1);

    private static UTF8Encoding encoding = new UTF8Encoding();

    public TcpClientN10()
    {

    }

    public TcpClientN10(IPAddress IPAddr, int Port)
    {
        this.IPAddr = IPAddr;
        this.Port = Port;
    }

    /// <summary>
    /// Connect to server
    /// </summary>
    /// <returns>awaitable task</returns>
    public async Task Connect()
    {
        if (client is null)
        {
            client = new TcpClient();
            await client.ConnectAsync(new IPEndPoint(IPAddr, Port)).ConfigureAwait(false); //Wait until connected
            tRead = TaskLongRunning.Run(() => StartReadAsync(client).WaitE()); //Starts a new tasks which reads incoming data
        }
    }

    /// <summary>
    /// Disconnects from server. This will raise a Disconnected event if client was still connected.
    /// </summary>
    /// <returns></returns>
    public async Task Disconnect()
    {
        cts.Cancel();
        if(tRead is not null && !tRead.IsCompleted)
        {
            await tRead;
        }
        cts.Dispose();
    }

    private async Task StartReadAsync(TcpClient client)
    {
        await ReadAsync(client).ConfigureAwait(false); //this method only returns when client is disconnected

        await writeSema.WaitAsync().ConfigureAwait(false); //make sure no one is accessing the client while we Dispose it.
        client.Close();
        client.Dispose();
        this.client = null; 
        writeSema.Release();
        Disconnected?.Invoke(this);
    }


    private async Task ReadAsync(TcpClient client)
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
                BytesReceived?.Invoke(this, buffer, dataLength);
                StringReceived?.Invoke(this, recvString);
                MessageN10? msg = MessageN10.TryDeserialize(recvString);
                if(msg is not null)
                {
                    MessageReceived?.Invoke(this, msg);
                }
            }
        }
        catch (OperationCanceledException) { }
    }

    /// <summary>
    /// Sends a Message to the server. Server should use MessageReceived event.
    /// </summary>
    /// <param name="msg">Message to send</param>
    /// <returns>awaitable Task</returns>
    public async Task SendMessage(MessageN10 msg)
    {
        if (client is not null)
        {
            await WriteString(msg.Serialize()).ConfigureAwait(false); //write json string of message
        }
    }

    /// <summary>
    /// Sends a string to the server. Server should use StringReceived event.
    /// </summary>
    /// <param name="text">String to send</param>
    /// <returns>awaitable Task</returns>
    public async Task WriteString(string text)
    {
        if (client is not null)
        {
            await WriteBytes(encoding.GetBytes(text)).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Sends a buffer to the server. Server should use BytesReceived event.
    /// </summary>
    /// <param name="buffer">Buffer to send</param>
    /// <returns>awaitable Task</returns>
    public async Task WriteBytes(byte[] buffer)
    {
        await writeSema.WaitAsync().ConfigureAwait(false);
        try
        {
            if (client is not null)
            {
                await client.GetStream().WriteAsync(BitConverter.GetBytes(buffer.Length)).ConfigureAwait(false);
                await client.GetStream().WriteAsync(buffer).ConfigureAwait(false);
            }
        }
        finally
        {
            writeSema.Release();
        }
    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network10Lib;

public class TcpClientAsync : ITcpConnectorAsync
{
    public delegate void MessageReceivedHandler(TcpClientAsync sender, string message);
    public event MessageReceivedHandler? MessageReceived;
    public delegate void MessageReceivedHandler2(TcpConnectionAsync.Message msg);
    public event MessageReceivedHandler2? Message2Received;
    public delegate void DisconnectedHandler(TcpClientAsync sender);
    public event DisconnectedHandler? Disconnected;

    TcpClient? client;
    CancellationTokenSource cts = new CancellationTokenSource();
    Task? tRead;

    public IPAddress IPAddr { get; init; } = IPAddress.Loopback;
    public int Port { get; init; } = 12345;

    private static UTF8Encoding encoding = new UTF8Encoding();

    public TcpClientAsync()
    {

    }

    public TcpClientAsync(IPAddress IPAddr, int Port)
    {
        this.IPAddr = IPAddr;
        this.Port = Port;
    }

    public async Task Connect()
    {
        if (client is null)
        {
            client = new TcpClient();
            await client.ConnectAsync(new IPEndPoint(IPAddr, Port)).ConfigureAwait(false);
            tRead = TaskLongRunning.Run(() => StartReadAsync(client).WaitE());
        }
    }

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
        await ReadAsync(client).ConfigureAwait(false);

        client.Close();
        client.Dispose();
        this.client = null;
        Disconnected?.Invoke(this);
    }


    public async Task ReadAsync(TcpClient client)
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
                MessageReceived?.Invoke(this, recvString);
                TcpConnectionAsync.Message? msg = TcpConnectionAsync.Message.TryDeserialize(recvString);
                if(msg is not null)
                {
                    Message2Received?.Invoke(msg);
                }
            }
        }
        catch (OperationCanceledException) { }
    }

    public async Task Write(string text)
    {
        if (client is not null)
        {
            byte[] buffer = encoding.GetBytes(text);
            await client.GetStream().WriteAsync(BitConverter.GetBytes(buffer.Length)).ConfigureAwait(false);
            await client.GetStream().WriteAsync(buffer).ConfigureAwait(false);
        }
    }

    public async Task Write(byte[] buffer)
    {
        if (client is not null)
        {
            await client.GetStream().WriteAsync(BitConverter.GetBytes(buffer.Length)).ConfigureAwait(false);
            await client.GetStream().WriteAsync(buffer).ConfigureAwait(false);
        }
    }

    public async Task SendMessage(TcpConnectionAsync.Message msg)
    {
        if (client is not null)
        {
            string s = msg.Serialize();
            byte[] buffer = encoding.GetBytes(s);
            await client.GetStream().WriteAsync(BitConverter.GetBytes(buffer.Length)).ConfigureAwait(false);
            await client.GetStream().WriteAsync(buffer).ConfigureAwait(false);
        }
    }
}


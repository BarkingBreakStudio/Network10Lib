using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network10Lib;

public class TcpClientAsync
{
    public delegate void MessageReceivedHandler(TcpClientAsync sender, string message);
    public event MessageReceivedHandler? MessageReceived;
    public delegate void DisconnectedHandler(TcpClientAsync sender);
    public event DisconnectedHandler? Disconnected;

    TcpClient? client;
    CancellationTokenSource cts = new CancellationTokenSource();
    Task? tRead;

    public async Task Connect()
    {
        if (client is null)
        {
            client = new TcpClient();
            await client.ConnectAsync(new IPEndPoint(IPAddress.Loopback, 12345)).ConfigureAwait(false);
            tRead = TaskLongRunning.Run(() => { StartReadAsync(client).Wait(); });
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
        UTF8Encoding encoding = new UTF8Encoding();
      
        try
        {
            int length;
            while ((length = await client.GetStream().ReadAsync(buffer, 0, buffer.Length, cts.Token).ConfigureAwait(false)) > 0)
            {
                MessageReceived?.Invoke(this, encoding.GetString(buffer, 0, length));
            }
        }
        catch (OperationCanceledException) { }
    }

    public async Task Write(string text)
    {
        if (client is not null)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            await client.GetStream().WriteAsync(encoding.GetBytes(text)).ConfigureAwait(false);
        }
    }

    public async Task Write(byte[] buffer)
    {
        if (client is not null)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            await client.GetStream().WriteAsync(buffer).ConfigureAwait(false);
        }
    }





}


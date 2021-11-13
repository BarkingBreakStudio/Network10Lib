using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network10Lib;
public class TcpServerAsync
{

    public delegate void MessageReceivedHandler(TcpServerAsync sender, int clientNr, TcpClient client, string message);
    public event MessageReceivedHandler? MessageReceived;
    public delegate void ClientConnectedHandler(TcpServerAsync sender, int clientNr, TcpClient client);
    public event ClientConnectedHandler? ClientConnected;
    public delegate void ClientDisconnectedHandler(TcpServerAsync sender, int clientNr, TcpClient client);
    public event ClientDisconnectedHandler? ClientDisconnected;

    TcpListener? tcpListener = null;

    CancellationTokenSource cts = new CancellationTokenSource();
    Task? tListen;
    List<Task> clientTasks = new List<Task>();
    List<TcpClient> clients = new List<TcpClient>();

    public void Start()
    {
        if (tcpListener is null)
        {
            tcpListener = new TcpListener(IPAddress.Any, 12345);
            tcpListener.Start();
            tListen = Task.Factory.StartNew(() => { AcceptClientAsync(tcpListener).Wait(); }, TaskCreationOptions.LongRunning);
        }
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
                int readlength = await client.GetStream().ReadAsync(buffer, 0, buffer.Length, cts.Token).ConfigureAwait(false);
                if (readlength > 0)
                {
                    MessageReceived?.Invoke(this, clientNr, client, new UTF8Encoding().GetString(buffer,0, readlength));
                    await client.GetStream().WriteAsync(buffer, 0, readlength).ConfigureAwait(false);
                }
                else
                {
                    break;
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
            byte[] buffer = new UTF8Encoding().GetBytes(text);
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
        byte[] buffer = new UTF8Encoding().GetBytes(text);
        await client.GetStream().WriteAsync(buffer, 0, buffer.Length);
    }

    public async Task Write(TcpClient client, byte[] buffer)
    {
        await client.GetStream().WriteAsync(buffer);
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
}


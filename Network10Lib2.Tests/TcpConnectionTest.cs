using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Network10Lib;

namespace Network10Lib2.Tests;

public class TcpConnectionTest
{

    public record Person
    {
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public int Age { get; set; }
    }

 

    [Fact]
    public async void TcpConnector_ConnectAndSend()
    {
        TcpConnectionAsync server = new TcpConnectionAsync();
        {
            GetEvent sc = new();
            sc.Init(() => server.Connected += sc.Callback, () => server.Connected -= sc.Callback);
            GetEventParam<int> pc = new();
            pc.Init(() => server.PlayerConnected += pc.Callback, () => server.PlayerConnected -= pc.Callback);

            await server.OpenServer();

            sc.WaitForEvent();
            int plNr = pc.WaitForEvent();
            Assert.Equal(0, plNr);
        }
        Assert.True(server.IsConnected);
        Assert.True(server.IsServer);

        TcpConnectionAsync client = new TcpConnectionAsync();
        {//get client conneced message
            GetEvent cc = new();
            cc.Init(() => client.Connected += cc.Callback, () => client.Connected -= cc.Callback);
            GetEventParam<int> pc = new();
            pc.Init(() => server.PlayerConnected += pc.Callback, () => server.PlayerConnected -= pc.Callback);

            await client.OpenClient();

            cc.WaitForEvent();
            int plNr = pc.WaitForEvent();
            Assert.Equal(1, plNr);
        }
        
        Assert.True(client.IsConnected);
        Assert.False(client.IsServer);


        {//send from client to server
            var gofc = new GetEventParam<TcpConnectionAsync.Message>();
            server.MessageReceived += gofc.Callback;
            Person p = new Person { Name = "Hans", Address = "Nizza", City = "La Palma", Age = 58 };
            await client.SendObject(p, 0);
            AssertEqual<Person>(new TcpConnectionAsync.Message { Sender = 1, Receiver = 0, MsgType = TcpConnectionAsync.Message.EnumMsgType.Tcp, Data = p }, gofc.WaitForEvent());
            server.MessageReceived -= gofc.Callback;
        }

        {//send from server to client
            var gofc = new GetEventParam<TcpConnectionAsync.Message>();
            client.MessageReceived += gofc.Callback;
            Person p = new Person { Name = "Robert", Address = "Bahamas", City = "Lama", Age = 45 };
            await server.SendObject(p, 1);
            AssertEqual<Person>(new TcpConnectionAsync.Message { Sender = 0, Receiver = 1, MsgType = TcpConnectionAsync.Message.EnumMsgType.Tcp, Data = p }, gofc.WaitForEvent());
            client.MessageReceived -= gofc.Callback;
        }

        {//send from server to itself
            var gofc = new GetEventParam<TcpConnectionAsync.Message>();
            server.MessageReceived += gofc.Callback;
            Person p = new Person { Name = "Lulu", Address = "Lemon", City = "Secret", Age = 5 };
            await server.SendObject(p, 0);
            AssertEqual<Person>(new TcpConnectionAsync.Message { Sender = 0, Receiver = 0, MsgType = TcpConnectionAsync.Message.EnumMsgType.Tcp, Data = p }, gofc.WaitForEvent());
            server.MessageReceived -= gofc.Callback;
        }

        {//send from client to itself
            var gofc = new GetEventParam<TcpConnectionAsync.Message>();
            client.MessageReceived += gofc.Callback;
            Person p = new Person { Name = "Tenscent", Address = "China", City = "Morescent", Age = 18 };
            await client.SendObject(p, 1);
            AssertEqual<Person>(new TcpConnectionAsync.Message { Sender = 1, Receiver = 1, MsgType = TcpConnectionAsync.Message.EnumMsgType.Tcp, Data = p }, gofc.WaitForEvent());
            client.MessageReceived -= gofc.Callback;
        }

        await client.Close();
        await server.Close(); 
    }


    [Fact]
    public async void TcpConnector_Disconnect()
    {
        //create server and client
        TcpConnectionAsync server = new TcpConnectionAsync();
        await server.OpenServer();

        TcpConnectionAsync client = new TcpConnectionAsync();
        await client.OpenClient();

        //disconnect client first: 
        GetEvent cd = new();
        client.Disonnected += cd.Callback;
        GetEventParam<int> pd = new();
        server.PlayerDisonnected += pd.Callback;

        await client.Close();
        Assert.False(client.IsConnected);

        cd.WaitForEvent();
        client.Disonnected -= cd.Callback;
        int plNr = pd.WaitForEvent();
        server.PlayerDisonnected -= pd.Callback;
        Assert.Equal(1, plNr);

        //connect with a new client
        client = new TcpConnectionAsync();
        await client.OpenClient();

        //disconnect server first
        GetEvent cd2 = new();
        client.Disonnected += cd2.Callback;
        GetEvent sd2 = new();
        server.Disonnected += sd2.Callback;
        GetEventParam<int> pd2 = new();
        server.PlayerDisonnected += pd2.Callback;

        await server.Close();
        Assert.False(server.IsConnected);
        Assert.False(client.IsConnected); //client gets disconnected too

        cd2.WaitForEvent();
        client.Disonnected -= cd2.Callback;
        sd2.WaitForEvent();
        server.Disonnected -= sd2.Callback;
        List<int> nDisPlayers = pd2.WaitForEvents();
        Assert.Equal(2, nDisPlayers.Count);
        Assert.Contains<int>(0, nDisPlayers);
        Assert.Contains<int>(2, nDisPlayers);

        //disconnecting client should not raise event
        GetEvent cd3 = new();
        client.Disonnected += cd3.Callback;

        await client.Close();
        cd3.WaitForEvent(false);
        client.Disonnected -= cd3.Callback;
    }


        private void AssertEqual<TmsgData>(TcpConnectionAsync.Message? expected, TcpConnectionAsync.Message? actual)
    {
        if (expected is null && actual is null)
            return;
        Assert.NotNull(expected);
        Assert.NotNull(actual);
        if (expected is not null && actual is not null)
        {
            Assert.Equal(expected.Sender, actual.Sender);
            Assert.Equal(expected.Receiver, actual.Receiver);
            Assert.Equal(expected.MsgType, actual.MsgType);
            Assert.Equal(expected.Data, actual.DeserializeData<TmsgData>());
        }
    }

    public class GetEventParam<T>
    {
        AutoResetEvent are = new AutoResetEvent(false);
        private Action? onEnd;
        List<T?> objs = new();
        int numEvents;

        public GetEventParam(int numEvents = 1)
        {
            this.numEvents = numEvents;
        }

        public void Init(Action onStart, Action onEnd)
        {
            are.Reset();
            onStart();
            this.onEnd = onEnd;
        }

        public void Callback(T obj)
        {
            objs.Add(obj);
            are.Set() ;
        }        

        public T? WaitForEvent(bool shouldSucceed = true, int msTimeout = 1000)
        {
            Assert.Equal(shouldSucceed, are.WaitOne(msTimeout));
            onEnd?.Invoke();
            return objs[0];
        }

        public List<T?> WaitForEvents(bool shouldSucceed = true, int msTimeout = 1000)
        {
            Assert.Equal(shouldSucceed, are.WaitOne(msTimeout));
            onEnd?.Invoke();
            return objs;
        }

    }

    public class GetEvent
    {
        AutoResetEvent are = new AutoResetEvent(false);
        private Action? onEnd;

        public void Init(Action onStart, Action onEnd)
        {
            are.Reset();
            onStart();
            this.onEnd = onEnd;
        }

        public void Callback()
        {
            are.Set();
        }

        public void WaitForEvent(bool shouldSucceed = true, int msTimeout = 1000)
        {
            Assert.Equal(shouldSucceed, are.WaitOne(msTimeout));
            onEnd?.Invoke();
        }

    }



}


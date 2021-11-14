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

    
    TcpConnectionAsync.Message expectedMsg = new TcpConnectionAsync.Message();
    //TcpConnectionAsync.Message? actualMsg;

    [Fact]
    public async void TcpConnector_SimpleTest()
    {
        TcpConnectionAsync server = new TcpConnectionAsync();
        await server.OpenServer();
        Assert.True(server.IsConnected);
        Assert.True(server.IsServer);

        TcpConnectionAsync client = new TcpConnectionAsync();
        await client.OpenClient();
        await Task.Delay(1000);
        Assert.True(client.IsConnected);
        Assert.False(client.IsServer);


        {//send from client to server
            var gofc = new GetObjectFromCallback<TcpConnectionAsync.Message>();
            server.MessageReceived += gofc.Callback;
            Person p = new Person { Name = "Hans", Address = "Nizza", City = "La Palma", Age = 58 };
            await client.SendObject(p, 0);
            AssertEqual<Person>(new TcpConnectionAsync.Message { Sender = 1, Receiver = 0, MsgType = TcpConnectionAsync.Message.EnumMsgType.Tcp, Data = p }, gofc.WaitForObj());
            server.MessageReceived -= gofc.Callback;
        }

        {//send from server to client
            var gofc = new GetObjectFromCallback<TcpConnectionAsync.Message>();
            client.MessageReceived += gofc.Callback;
            Person p = new Person { Name = "Robert", Address = "Bahamas", City = "Lama", Age = 45 };
            await server.SendObject(p, 1);
            AssertEqual<Person>(new TcpConnectionAsync.Message { Sender = 0, Receiver = 1, MsgType = TcpConnectionAsync.Message.EnumMsgType.Tcp, Data = p }, gofc.WaitForObj());
            client.MessageReceived -= gofc.Callback;
        }

        {//send from server to itself
            var gofc = new GetObjectFromCallback<TcpConnectionAsync.Message>();
            server.MessageReceived += gofc.Callback;
            Person p = new Person { Name = "Lulu", Address = "Lemon", City = "Secret", Age = 5 };
            await server.SendObject(p, 0);
            AssertEqual<Person>(new TcpConnectionAsync.Message { Sender = 0, Receiver = 0, MsgType = TcpConnectionAsync.Message.EnumMsgType.Tcp, Data = p }, gofc.WaitForObj());
            server.MessageReceived -= gofc.Callback;
        }

        {//send from client to itself
            var gofc = new GetObjectFromCallback<TcpConnectionAsync.Message>();
            client.MessageReceived += gofc.Callback;
            Person p = new Person { Name = "Tenscent", Address = "China", City = "Morescent", Age = 18 };
            await client.SendObject(p, 1);
            AssertEqual<Person>(new TcpConnectionAsync.Message { Sender = 1, Receiver = 1, MsgType = TcpConnectionAsync.Message.EnumMsgType.Tcp, Data = p }, gofc.WaitForObj());
            client.MessageReceived -= gofc.Callback;
        }



        await client.Close();
        await server.Close();

       
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

    public class GetObjectFromCallback<T>
    {
        AutoResetEvent are = new AutoResetEvent(false);

        private T? obj;


        public void Callback(T obj)
        {
            this.obj = obj;
            are.Set() ;
        }        

        public T? WaitForObj(bool shouldSucceed = true, int msTimeout = 1000)
        {
            Assert.Equal(shouldSucceed, are.WaitOne(msTimeout));
            return obj;
        }

    }

}


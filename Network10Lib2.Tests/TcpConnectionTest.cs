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
    [Fact]
    public async void TcpConnector_SimpleTest()
    {
        TcpConnectionAsync server = new TcpConnectionAsync();
        await server.OpenServer();

        Assert.True(server.IsConnected);
        Assert.True(server.IsServer);

        TcpConnectionAsync client = new TcpConnectionAsync();
        await client.OpenClient();

        while (true) ;

        Assert.True(client.IsConnected);
        Assert.False(client.IsServer);

        await client.Close();
        await server.Close();

       
    }

}


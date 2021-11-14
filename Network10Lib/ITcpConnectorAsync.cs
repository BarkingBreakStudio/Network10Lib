using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network10Lib
{
    internal interface ITcpConnectorAsync
    {

        public Task Connect();
        public Task SendMessage(TcpConnectionAsync.Message msg);
        public Task Disconnect();
    }
}

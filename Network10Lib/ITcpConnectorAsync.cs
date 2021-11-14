using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network10Lib
{
    public interface ITcpConnectorAsync
    {

        public Task Connect();
        public Task SendMessage(TcpConnectionAsync.Message msg);
        public Task Disconnect();
    }
}

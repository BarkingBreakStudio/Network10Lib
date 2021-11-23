using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network10Lib
{
    /// <summary>
    /// ITcpConnectorN10 represents a TcpServerN10 or a TcoClientN10
    /// </summary>
    public interface ITcpConnectorN10
    {
        /// <summary>
        /// Calls Connect on server or client
        /// </summary>
        /// <returns>awaitable task</returns>
        public Task Connect();
        /// <summary>
        /// Calls SendMessage on server or client
        /// </summary>
        /// <param name="msg">sends a message</param>
        /// <returns>awaitable Task</returns>
        public Task SendMessage(MessageN10 msg);
        /// <summary>
        /// Calls Disconnect on server or client.
        /// </summary>
        /// <returns>awaitable Task</returns>
        public Task Disconnect();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network10Lib
{
    internal static class Extensions
    {

        internal static async Task ReadUntilLengthAsync(this NetworkStream stream, byte[] outBuffer, int length, CancellationToken cancellationToken)
        {
            int nNewRead = 0;
            int nRead = 0;
            do
            {
                nNewRead = await stream.ReadAsync(outBuffer, nRead, length - nRead, cancellationToken).ConfigureAwait(false);
                if (nNewRead == 0) { throw new OperationCanceledException(); }
                nRead += nNewRead;
            } while (nRead != length);
        }

    }
}

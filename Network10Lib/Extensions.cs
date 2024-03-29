﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network10Lib
{
    /// <summary>
    /// Internal extension functions
    /// </summary>
    internal static class Extensions
    {

        /// <summary>
        /// Reads from Networkstream until requested read length is reached
        /// </summary>
        /// <param name="stream">Networkstream</param>
        /// <param name="outBuffer">byte buffer with a minimal length of the requested read length</param>
        /// <param name="length">requested read length</param>
        /// <param name="cancellationToken">cancels this function</param>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException"></exception>
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

        /// <summary>
        /// Waits as Wait() but unwraps Errors in case the thask throws an error
        /// </summary>
        /// <param name="t">Task to wait for</param>
        internal static void WaitE(this Task t)
        {
            t.GetAwaiter().GetResult();
        }

    }
}

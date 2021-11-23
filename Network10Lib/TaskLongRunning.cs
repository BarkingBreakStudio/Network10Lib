using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network10Lib;

/// <summary>
/// Internal helper function
/// </summary>
internal static class TaskLongRunning
{
    /// <summary>
    /// Starts a new Tasks which does not consume a thread from the threadpool. 
    /// </summary>
    /// <param name="action">Action perfo0mred by the new Task</param>
    /// <returns>awaitable Task</returns>
    public static Task Run(Action action)
    {
        return Task.Factory.StartNew(action, TaskCreationOptions.LongRunning);
    }

}



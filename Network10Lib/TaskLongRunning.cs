using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network10Lib;

internal static class TaskLongRunning
{


    public static Task Run(Action action)
    {
        return Task.Factory.StartNew(action, TaskCreationOptions.LongRunning);
    }

}



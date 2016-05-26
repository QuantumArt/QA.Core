using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QA.Core
{
    public static class ILoggerExtensions
    {
        public static ILogger LogDebug(this ILogger logger, string msg, [CallerMemberName] string caller = "")
        {
            if (logger == null)
                return null;

            logger.Debug(x => string.Format("{2} Logging for method {0}: {1}", caller, msg, Thread.CurrentThread.ManagedThreadId));

            return logger;
        }

        public static ILogger LogDebug(this ILogger logger, Func<string> msg, [CallerMemberName] string caller = "")
        {
            if (logger == null)
                return null;

            logger.Debug(x => string.Format("{2} Logging for method {0}: {1}", caller, msg(), Thread.CurrentThread.ManagedThreadId));

            return logger;
        }
    }
}

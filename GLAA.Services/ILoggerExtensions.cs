using Microsoft.Extensions.Logging;
using System;

namespace GLAA.Services.Extensions
{
    public static class ILoggerExtensions
    {
        public static void LogWithTimestamp(this ILogger logger, LogLevel level, string message, Exception exception = null)
        {
            logger.Log(level, 1, message, exception, (s, e) => DateTime.Now + " " + s.ToString());
        }
    }
}

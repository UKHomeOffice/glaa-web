using Microsoft.Extensions.Logging;
using System;

namespace GLAA.Services.Extensions
{
    public static class ILoggerExtensions
    {
        public static void TimedLog<T>(this ILogger<T> logger, LogLevel level, string message, Exception exception = null)
        {
            logger.Log(level, 1, message, exception, (s, e) => $"{s}, {e}");
        }
    }
}

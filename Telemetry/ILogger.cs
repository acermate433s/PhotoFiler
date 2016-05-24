using System;
using System.Diagnostics;

namespace Telemetry
{
    /// <summary>
    /// Based on http://stackoverflow.com/questions/5646820/logger-wrapper-best-practice
    /// </summary>
    public interface ILogger
    {
        void Log(LogEntry entry);
    }

    public class LogEntry
    {
        public readonly TraceEventType Severity;
        public readonly string Message;
        public readonly int ID = 0;
        public readonly object[] Datum = null;
        public readonly Exception Exception = null;

        public LogEntry(
            TraceEventType severity,
            string message = null, 
            int id = 0,
            object[] datum = null,
            Exception exception = null
        )
        {
            this.Severity = severity;
            this.Message = message;
            this.ID = id;
            this.Datum = datum;
            this.Exception = exception;
        }
    }

    public static class ILoggerExtensions
    {
        public static void Critical(this ILogger logger, string message, int id = 0, Exception exception = null)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Critical, 
                    message, 
                    id,
                    exception: exception
                )
            );
        }

        public static void Critical(this ILogger logger, string message, int id = 0, Exception exception = null, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Critical, 
                    message, 
                    id, 
                    datum,
                    exception
                )
            );
        }

        public static void Critical(this ILogger logger, Exception exception, object data = null)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Critical,
                    datum: new object[] { data },
                    exception: exception
                )
            );
        }

        public static void Critical(this ILogger logger, Exception exception, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Critical, 
                    datum: datum,
                    exception: exception
                )
            );
        }

        public static void Error(this ILogger logger, string message, int id = 0, Exception exception = null)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Error,
                    message,
                    id,
                    exception: exception
                )
            );
        }

        public static void Error(this ILogger logger, string message, int id = 0, Exception exception = null, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Error,
                    message,
                    id,
                    datum,
                    exception
                )
            );
        }

        public static void Error(this ILogger logger, Exception exception, object data = null)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Error,
                    datum: new object[] { data },
                    exception: exception
                )
            );
        }

        public static void Error(this ILogger logger, Exception exception, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Error,
                    datum: datum,
                    exception: exception
                )
            );
        }

        public static void Information(this ILogger logger, string message, int id = 0)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Information,
                    message,
                    id
                )
            );
        }

        public static void Information(this ILogger logger, string message, int id = 0, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Information,
                    message,
                    id,
                    datum
                )
            );
        }

        public static void Information(this ILogger logger, object data)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Information,
                    datum: new object[] { data }
                )
            );
        }

        public static void Information(this ILogger logger, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Information,
                    datum: datum
                )
            );
        }

        public static void Verbose(this ILogger logger, string message, int id = 0)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Verbose,
                    message,
                    id
                )
            );
        }

        public static void Verbose(this ILogger logger, string message, int id = 0, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Verbose,
                    message,
                    id,
                    datum
                )
            );
        }

        public static void Verbose(this ILogger logger, object data)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Information,
                    datum: new object[] { data }
                )
            );
        }

        public static void Verbose(this ILogger logger, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Information,
                    datum: datum
                )
            );
        }

        public static void Warning(this ILogger logger, string message, int id = 0)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Warning,
                    message,
                    id
                )
            );
        }

        public static void Warning(this ILogger logger, string message, int id = 0, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Warning,
                    message,
                    id,
                    datum
                )
            );
        }

        public static void Warning(this ILogger logger, object data)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Warning,
                    datum: new object[] { data }
                )
            );
        }

        public static void Warning(this ILogger logger, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    TraceEventType.Warning,
                    datum: datum
                )
            );
        }
    }
}

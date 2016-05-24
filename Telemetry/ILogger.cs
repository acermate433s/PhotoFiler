using System;
using System.Diagnostics;

namespace Telemetry
{
    /// <summary>
    /// Based on http://stackoverflow.com/questions/5646820/logger-wrapper-best-practice
    /// </summary>
    public enum Severity
    {
        Critical = TraceEventType.Critical,
        Error = TraceEventType.Error,
        Information = TraceEventType.Information,
        Verbose = TraceEventType.Verbose,
        Warning = TraceEventType.Warning,
    }

    public interface ILogger
    {
        void Log(LogEntry entry);
    }

    public class LogEntry
    {
        public readonly Severity Severity;
        public readonly string Message;
        public readonly int ID = 0;
        public readonly object[] Datum = null;
        public readonly Exception Exception = null;

        public LogEntry(
            Severity severity,
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
                    Severity.Critical, 
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
                    Severity.Critical, 
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
                    Severity.Critical,
                    datum: new object[] { data },
                    exception: exception
                )
            );
        }

        public static void Critical(this ILogger logger, Exception exception, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    Severity.Critical, 
                    datum: datum,
                    exception: exception
                )
            );
        }

        public static void Error(this ILogger logger, string message, int id = 0, Exception exception = null)
        {
            logger.Log(
                new LogEntry(
                    Severity.Error,
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
                    Severity.Error,
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
                    Severity.Error,
                    datum: new object[] { data },
                    exception: exception
                )
            );
        }

        public static void Error(this ILogger logger, Exception exception, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    Severity.Error,
                    datum: datum,
                    exception: exception
                )
            );
        }

        public static void Information(this ILogger logger, string message, int id = 0)
        {
            logger.Log(
                new LogEntry(
                    Severity.Information,
                    message,
                    id
                )
            );
        }

        public static void Information(this ILogger logger, string message, int id = 0, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    Severity.Information,
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
                    Severity.Information,
                    datum: new object[] { data }
                )
            );
        }

        public static void Information(this ILogger logger, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    Severity.Information,
                    datum: datum
                )
            );
        }

        public static void Verbose(this ILogger logger, string message, int id = 0)
        {
            logger.Log(
                new LogEntry(
                    Severity.Verbose,
                    message,
                    id
                )
            );
        }

        public static void Verbose(this ILogger logger, string message, int id = 0, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    Severity.Verbose,
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
                    Severity.Information,
                    datum: new object[] { data }
                )
            );
        }

        public static void Verbose(this ILogger logger, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    Severity.Information,
                    datum: datum
                )
            );
        }

        public static void Warning(this ILogger logger, string message, int id = 0)
        {
            logger.Log(
                new LogEntry(
                    Severity.Warning,
                    message,
                    id
                )
            );
        }

        public static void Warning(this ILogger logger, string message, int id = 0, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    Severity.Warning,
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
                    Severity.Warning,
                    datum: new object[] { data }
                )
            );
        }

        public static void Warning(this ILogger logger, params object[] datum)
        {
            logger.Log(
                new LogEntry(
                    Severity.Warning,
                    datum: datum
                )
            );
        }
    }
}

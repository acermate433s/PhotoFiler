using System;
using System.Diagnostics;

namespace Telemetry
{
    /// <summary>
    /// Base on http://www.codeproject.com/Articles/185666/ActivityTracerScope-Easy-activity-tracing-with
    /// </summary>
    public class ActivityTracerScope : IDisposable
    {
        /// <summary>
        /// Previous ID before TransferTrace
        /// </summary>
        protected Guid PreviousID { get; set; }

        /// <summary>
        /// ID for the current TransferTrace
        /// </summary>
        protected Guid CurrentID { get; set; }

        /// <summary>
        /// Current TraceSource 
        /// </summary>
        public TraceSource TraceSource { get; protected set; }

        /// <summary>
        /// User-defined ID for the activity
        /// </summary>
        public int ActivityID { get; protected set; }

        /// <summary>
        /// User-defined name for the activity
        /// </summary>
        public string ActivityName { get; protected set; }

        /// <summary>
        /// </summary>
        /// <param name="traceSource">TraceSource to use to log activity</param>
        /// <param name="activityName">User-defined name for the current activity</param>
        /// <param name="activityID">User-defined id for the current activity</param>
        public ActivityTracerScope(
            TraceSource traceSource,
            string activityName = "",
            int activityID = 0
        ) 
        {   
            TraceSource = traceSource;
            ActivityID = activityID;
            ActivityName = activityName;

            // remember the previous activity ID so we could come back to it 
            // later when we switch back to the previous activity before this
            PreviousID = Trace.CorrelationManager.ActivityId;

            // create a new ID for the current activity; we would need this 
            // when we when call TraceEvent with TraceEventType.Stop
            CurrentID = Guid.NewGuid();

            // transfer to a new activity and then start the trace event
            if (PreviousID != Guid.Empty)
                TraceSource.TraceTransfer(ActivityID, "Transferring to new activity", CurrentID);
            Trace.CorrelationManager.ActivityId = CurrentID;
            TraceSource.TraceEvent(TraceEventType.Start, ActivityID, ActivityName);
        }

        /// <summary>
        /// Creates a new TraceSource instead of using a user-defined one.
        /// </summary>
        /// <param name="traceName">Name for the soon-to-be created TraceSource to use to log activity</param>
        /// <param name="activityName">User-defined name for the current activity</param>
        /// <param name="activityID">User-defined id for the current activity</param>
        public ActivityTracerScope(
            string traceName,
            string activityName = "",
            int activityID = 0
        ) : this(new TraceSource(traceName), activityName, activityID)
        {
        }

        /// <summary>
        /// Transfer to the previous activity and then stop the current trace event
        /// </summary>
        public void Dispose()
        {
            if (PreviousID != Guid.Empty)
                TraceSource.TraceTransfer(ActivityID, "Transferring back to previous activity", PreviousID);
            TraceSource.TraceEvent(TraceEventType.Stop, ActivityID, ActivityName);
            Trace.CorrelationManager.ActivityId = PreviousID;
        }

        #region " TraceSource method short cuts "

        public void Data(TraceEventType eventType, object data)
        {
            TraceSource.TraceData(eventType, ActivityID, data);
        }

        public void Data(TraceEventType eventType, params object[] data)
        {
            TraceSource.TraceData(eventType, ActivityID, data);
        }

        private void Event(TraceEventType eventType)
        {
            TraceSource.TraceEvent(eventType, ActivityID);
        }

        private void Event(TraceEventType eventType, string message)
        {
            TraceSource.TraceEvent(eventType, ActivityID, message);
        }

        private void Event(TraceEventType eventType, string message, params object[] args)
        {
            TraceSource.TraceEvent(eventType, ActivityID, message, args);
        }

        public void Critical(string message)
        {
            Event(TraceEventType.Critical, message);
        }

        public void Critical(string message, params object[] args)
        {
            Event(TraceEventType.Critical, message, args);
        }

        public void Error(string message)
        {
            Event(TraceEventType.Error, message);
        }

        public void Error(string message, params object[] args)
        {
            Event(TraceEventType.Error, message, args);
        }

        public void Warning(string message)
        {
            Event(TraceEventType.Warning, message);
        }

        public void Warning(string message, params object[] args)
        {
            Event(TraceEventType.Warning, message, args);
        }

        public void Information(string message)
        {
            TraceSource.TraceInformation(message);
        }

        public void Information(string message, params object[] args)
        {
            TraceSource.TraceInformation(message, args);
        }

        public void Verbose(string message)
        {
            Event(TraceEventType.Verbose, message);
        }

        public void Verbose(string message, params object[] args)
        {
            Event(TraceEventType.Verbose, message, args);
        }

        #endregion
    }
}

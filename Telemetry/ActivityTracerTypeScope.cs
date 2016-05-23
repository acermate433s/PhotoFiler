using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Telemetry
{
    /// <summary>
    /// Base on http://www.codeproject.com/Articles/185666/ActivityTracerScope-Easy-activity-tracing-with
    /// </summary>
    public class ActivityTracerTypeScope<ActivityEnumType> : ActivityTracerScope where ActivityEnumType : struct, IConvertible, IComparable, IFormattable
    {
        public readonly ActivityEnumType Activity;

        public new int ActivityID
        {
            get
            {
                return Activity.ToInt32(null);
            }
        }

        public new string ActivityName
        {
            get
            {
                return GetDescription(Activity);
            }
        }

        private static string GetDescription(
            ActivityEnumType activityType
        )
        {
            // default would be the ToString() method of the ActivityEnum
            // if we cannot find a DescriptionAttribute
            var result = activityType.ToString();

            // get the DescriptionAttribute of the ActivityEnum and
            // use the Description property as the return value
            var fieldInfo = activityType.GetType().GetField(result);
            var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attributes.Length > 0 && attributes[0] is DescriptionAttribute)
            {
                var description = (DescriptionAttribute) attributes[0];
                result = description.Description;
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="traceSource">TraceSource to use to log activity</param>
        /// <param name="activityType">Activity type of the current activity</param>
        public ActivityTracerTypeScope(
            TraceSource traceSource,
            ActivityEnumType activityType = default(ActivityEnumType)
        ) : base(
                traceSource, 
                GetDescription(activityType), 
                activityType.ToInt32(null)
        )
        {
            if (!typeof(ActivityEnumType).IsEnum)
                throw new NotSupportedException($"{nameof(ActivityEnumType)} must be an enumerated type");

            Activity = activityType;

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
        /// <param name="traceName">Activity name of the current activity</param>
        /// <param name="activityType">Activity type of the current activity</param>
        public ActivityTracerTypeScope(
            string traceName,
            ActivityEnumType activityType = default(ActivityEnumType)
        ) : this(new TraceSource(traceName), activityType)
        {
        }
    }
}

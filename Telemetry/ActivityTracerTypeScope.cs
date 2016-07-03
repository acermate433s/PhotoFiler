using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Telemetry
{
    /// <summary>
    /// Base on http://www.codeproject.com/Articles/185666/ActivityTracerScope-Easy-activity-tracing-with
    /// </summary>
    public class ActivityTracerScope<ActivityEnumType> : ActivityTracerScope where ActivityEnumType : struct, IConvertible, IComparable, IFormattable
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

        static ActivityTracerScope()
        {
            if (!typeof(ActivityEnumType).IsEnum)
                throw new NotSupportedException($"{nameof(ActivityEnumType)} must be an enumerated type");
        }

        /// <summary>
        /// </summary>
        /// <param name="traceSource">TraceSource to use to log activity</param>
        /// <param name="activityType">Activity type of the current activity</param>
        public ActivityTracerScope(
            TraceSource traceSource,
            ActivityEnumType activityType = default(ActivityEnumType)
        ) : base(
                traceSource,
                GetDescription(activityType),
                activityType.ToInt32(null)
        )
        {
            Activity = activityType;
        }

        /// <summary>
        /// Creates a new TraceSource instead of using a user-defined one.
        /// </summary>
        /// <param name="traceName">Activity name of the current activity</param>
        /// <param name="activityType">Activity type of the current activity</param>
        public ActivityTracerScope(
            string traceName,
            ActivityEnumType activityType = default(ActivityEnumType)
        ) : this(new TraceSource(traceName), activityType)
        {
        }
    }
}
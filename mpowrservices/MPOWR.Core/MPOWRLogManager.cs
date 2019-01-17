using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using System.Diagnostics;

namespace MPOWR.Core
{
    public class MPOWRLogManager
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MPOWRLogManager));
        public static void LogMessage(string msg)
        {
            logger.Info(msg);
        }
        public static void LogException(string msg)
        {
            logger.Error(msg);
        }

    }

    public class AzureTraceAppender : AppenderSkeleton
    {
        /// <summary>
        /// The category
        /// </summary>
        private PatternLayout category = new PatternLayout("%logger");

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public PatternLayout Category
        {
            get { return this.category; }
            set { this.category = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [immediate flush].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [immediate flush]; otherwise, <c>false</c>.
        /// </value>
        public bool ImmediateFlush { get; set; }

        /// <summary>
        /// Subclasses of <see cref="T:log4net.Appender.AppenderSkeleton" /> should implement this method
        /// to perform actual logging.
        /// </summary>
        /// <param name="loggingEvent">The event to append.</param>
        /// <remarks>
        /// <para>
        /// A subclass must implement this method to perform
        /// logging of the <paramref name="loggingEvent" />.
        /// </para>
        /// <para>This method will be called by <see cref="M:DoAppend(LoggingEvent)" />
        /// if all the conditions listed for that method are met.
        /// </para>
        /// <para>
        /// To restrict the logging of events in the appender
        /// override the <see cref="M:PreAppendCheck()" /> method.
        /// </para>
        /// </remarks>
        protected override void Append(LoggingEvent loggingEvent)
        {
            string logMessage = string.Format(this.RenderLoggingEvent(loggingEvent), ((LayoutSkeleton)this.category).Format(loggingEvent));

            if (loggingEvent.Level == Level.Alert ||
                loggingEvent.Level == Level.Critical ||
                loggingEvent.Level == Level.Emergency ||
                loggingEvent.Level == Level.Error ||
                loggingEvent.Level == Level.Fatal ||
                loggingEvent.Level == Level.Log4Net_Debug ||
                loggingEvent.Level == Level.Severe)
            {
                Trace.TraceError(logMessage);
            }
            else if (loggingEvent.Level == Level.Warn)
            {
                Trace.TraceWarning(logMessage);
            }
            else if (loggingEvent.Level == Level.Debug ||
                loggingEvent.Level == Level.Fine ||
                loggingEvent.Level == Level.Finer ||
                loggingEvent.Level == Level.Finest ||
                loggingEvent.Level == Level.Info ||
                loggingEvent.Level == Level.Notice ||
                loggingEvent.Level == Level.Trace ||
                loggingEvent.Level == Level.Verbose)
            {
                Trace.TraceInformation(logMessage);
            }

            if (!this.ImmediateFlush)
            {
                return;
            }

            Trace.Flush();
        }
    }
}

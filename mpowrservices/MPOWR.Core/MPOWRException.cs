using System;
using System.Runtime.Serialization;

namespace MPOWR.Core
{
    [Serializable]
    public class MPOWRException : Exception
    {
        public MPOWRException()
            : base() { }

        public MPOWRException(string message)
            : base(message) { }

        public MPOWRException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public MPOWRException(string message, Exception innerException)
            : base(message, innerException) { }

        public MPOWRException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected MPOWRException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}

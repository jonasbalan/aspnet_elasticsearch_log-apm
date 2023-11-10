using System.Runtime.Serialization;

namespace SamplesApi3.Exception
{
    [Serializable]
    internal class StateScheduleInvalidException : System.Exception
    {
        const string msg = "State not valid to this schedule";

        public StateScheduleInvalidException() : base(msg)
        {
        }

        public StateScheduleInvalidException(string? message) : base(message)
        {
        }

        public StateScheduleInvalidException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        protected StateScheduleInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
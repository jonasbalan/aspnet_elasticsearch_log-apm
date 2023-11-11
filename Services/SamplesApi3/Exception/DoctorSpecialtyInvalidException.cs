using System.Runtime.Serialization;

namespace SamplesApi3.Exception
{
    [Serializable]
    internal class DoctorSpecialtyInvalidException : System.Exception
    {
        const string msg = "Doctor specialty not valid";
        public DoctorSpecialtyInvalidException() : base(msg)
        {
        }

        public DoctorSpecialtyInvalidException(string? message) : base(message)
        {
        }

        public DoctorSpecialtyInvalidException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        protected DoctorSpecialtyInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
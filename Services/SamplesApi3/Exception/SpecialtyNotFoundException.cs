using System.Runtime.Serialization;

namespace SamplesApi3.Exception
{
    [Serializable]
    internal class SpecialtyNotFoundException : System.Exception
    {
        public SpecialtyNotFoundException()
        {
        }

        public SpecialtyNotFoundException(string? message) : base(message)
        {
        }

        public SpecialtyNotFoundException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        protected SpecialtyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
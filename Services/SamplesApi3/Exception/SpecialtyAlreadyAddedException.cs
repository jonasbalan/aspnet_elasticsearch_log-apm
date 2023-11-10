namespace SamplesApi3.Exception
{
    public class SpecialtyAlreadyAddedException : System.Exception
    {
        const string msg = "Specialty already added!";

        public SpecialtyAlreadyAddedException(): base(msg)
        {
            
        }

    }
}

namespace SamplesApi3.Model
{
    public class DoctorSpecialty
    {

        public DoctorSpecialty() { }
        public DoctorSpecialty SetValues(Doctor doctor, string specialty)
        {
            Doctor = doctor;
            Specialty = specialty;

            return this;
        }


        public Guid Id { get; set; }
        public Doctor Doctor { get; set; }

        public string Specialty { get; set; }
    }
}

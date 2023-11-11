using SamplesApi3.Model.Enum;

namespace SamplesApi3.Model
{
    public class DoctorSchedule
    {
        public Guid Id { get; set; }
        public Doctor Doctor { get; set; }
        public ICollection<DoctorSpecialty> DoctorSpecialties { get; set; }           
        public DoctorAddress Address { get; set; }
        public  DateTime DateInit { get; set; }
        public DateTime DateEnd { get; set; }
        public StateScheduleEnum State { get; set; }

    }
}

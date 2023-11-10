using SamplesApi3.Model.Enum;

namespace SamplesApi3.Model
{
    public class DoctorSchedulePatient
    {
        public DoctorSchedulePatient() { }

        public DoctorSchedulePatient SetValues(DoctorSchedule doctorSchedule, Patient patient, DoctorSpecialty specialty, StateScheduleEnum state)
        {
            Id = Guid.NewGuid();
            DoctorSchedule = doctorSchedule;
            Patient = patient;
            Specialty = specialty;
            State = state;

            ValidateAppointment(doctorSchedule, specialty, state);

            return this;
        }

        private static void ValidateAppointment(DoctorSchedule doctorSchedule, DoctorSpecialty specialty, StateScheduleEnum state)
        {
            var isvalidScheduleSpecialty = doctorSchedule.DoctorSpecialties.Any(x => x.Doctor == specialty.Doctor &&
            x.Specialty.Equals(specialty.Specialty, StringComparison.CurrentCultureIgnoreCase));

            if (isvalidScheduleSpecialty)
            {
                if (!ValidateState(doctorSchedule.State, state))
                    throw new Exception.StateScheduleInvalidException();
            }
            else
                throw new Exception.DoctorSpecialtyInvalidException();
        }

        public Guid Id { get; set; }

        public DoctorSchedule DoctorSchedule { get; set; }

        public Patient Patient { get; set; }

        public StateScheduleEnum State { get; private set; }

        public DoctorSpecialty Specialty { get; set; }

        private static bool ValidateState(StateScheduleEnum doctorSchedule, StateScheduleEnum newState)
        {
            switch (doctorSchedule)
            {
                case StateScheduleEnum.Scheduled: return newState == StateScheduleEnum.Executed;

                case StateScheduleEnum.Executed: return false;

                case StateScheduleEnum.Free: return newState == StateScheduleEnum.Scheduled;

                case StateScheduleEnum.Blocked: return false;

                default: return false;
            }

        }

        internal void SetState(StateScheduleEnum state)
        {
            if (ValidateState(this.State, state))
            {
                this.State = state;
            }
            else
                throw new Exception.StateScheduleInvalidException();
        }
    }
}

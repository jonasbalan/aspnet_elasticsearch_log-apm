using SamplesApi3.Model.Enum;

namespace SamplesApi3.Model
{
    public class Doctor
    {

        public Doctor()
        {
            this.Specialties = new List<DoctorSpecialty>();
            this.Schedules = new List<DoctorSchedule>();
            this.SchedulePatients = new List<DoctorSchedulePatient>();
            this.Adresses = new List<DoctorAddress>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Register { get; set; }
        public ICollection<DoctorSpecialty> Specialties { get; set; }
        public ICollection<DoctorAddress> Adresses { get; set; }
        public ICollection<DoctorSchedule> Schedules { get; set; }
        public ICollection<DoctorSchedulePatient> SchedulePatients { get; set; }


        // -------------------------------- Doctor Specialties -----------------------------------------------------

        private static Func<DoctorSpecialty, string, bool> DoctorEspecialtySearchExpression = (x, specialty) => x.Specialty.Equals(specialty, StringComparison.CurrentCultureIgnoreCase);

        private static Func<DoctorSpecialty, string[], bool> DoctorEspecialtyWhereExpression = (x, specialties) => specialties.Any(specialty => DoctorEspecialtySearchExpression(x, specialty));

        public Doctor AddSpecialties(string specialty)
        {
            if (!ThereIsSpecialty(specialty))
            {
                var specObj = new DoctorSpecialty().SetValues(this, specialty);

                Specialties.Add(specObj);
            }
            else
                throw new Exception.SpecialtyAlreadyAddedException();

            return this;
        }

        public bool ThereIsSpecialty(string specialty)
        {
            return Specialties.Any(x => DoctorEspecialtySearchExpression(x, specialty));
        }
        public bool ThereAreSpecialties(params string[] specialties)
        {
            return Specialties.Any(x => DoctorEspecialtyWhereExpression(x, specialties));
        }
        public IEnumerable<DoctorSpecialty> GetSpecialties(params string[] specialties)
        {
            return Specialties.Where(x => DoctorEspecialtyWhereExpression(x, specialties));
        }

        public Doctor RemoveSpecialties(string specialty)
        {
            if (ThereIsSpecialty(specialty))
            {
                var specObj = new DoctorSpecialty().SetValues(this, specialty);
                Specialties.Remove(specObj);
            }
            else
                throw new Exception.SpecialtyNotFoundException();

            return this;
        }

        // -------------------------------- Doctor Address -----------------------------------------------------

        public Doctor AddAddress(string street, string number, string neighborhood, string city, string state, string zipCode, string complement, string country, string name)
        {
            // have to do Address validations 
            this.Adresses.Add(new DoctorAddress(street, number, neighborhood, city, state, zipCode, complement, country, name));

            return this;
        }

        private DoctorAddress GetAddress(DoctorAddress address)
        {
            return this.Adresses.FirstOrDefault(x => x.Id == address.Id);
        }

        private bool ThereIsAddress(DoctorAddress doctorAddress)
        {
            return this.Adresses.Any(x => x.Id == doctorAddress.Id);
        }


        // ----------------------------- Doctor Schedule  ----------------------------------------------------------

        public Doctor AddSchedule(DateTime date, int duration, StateScheduleEnum state, DoctorAddress address, params string[] specialties)
        {
            DateTime dateEnd = date.AddMinutes(duration);

            if (!ThereIsSchedule(date, dateEnd) && ThereAreSpecialties(specialties) && ThereIsAddress(address))
            {
                Schedules.Add(new DoctorSchedule()
                {
                    DateInit = date,
                    DateEnd = dateEnd,
                    Doctor = this,
                    DoctorSpecialties = GetSpecialties(specialties).ToList(),
                    State = state,
                    Address = GetAddress(address)
                });
            }

            return this;
        }

        private bool ThereIsSchedule(DateTime dateInit, DateTime dateEnd)
        {
            return Schedules.Any(x =>
            {
                var result =
                x.DateInit <= dateInit &&
                x.DateEnd >= dateInit &&
                x.DateInit <= dateEnd &&
                x.DateInit >= dateEnd;
                return result;
            });
        }

        private Doctor UpdateScheduleState(DoctorSchedule schedule, StateScheduleEnum state)
        {
            Func<DoctorSchedule, bool> expresion = x => x.DateInit.Equals(schedule.DateInit) && x.DateEnd.Equals(schedule.DateEnd);
            var foundedSchedule = this.Schedules.FirstOrDefault(expresion);
            if (foundedSchedule != null)
                foundedSchedule.State = state;

            return this;
        }

        // --------------------------------- Doctor schedule Patient ---------------------------------------------------


        public Doctor SchedulePatientAppointment(Patient patient, DoctorSchedule doctorSchedule, DoctorSpecialty specialty)
        {
            var appointment = new DoctorSchedulePatient().SetValues(doctorSchedule, patient, specialty, StateScheduleEnum.Scheduled);
            this.SchedulePatients.Add(appointment);
            UpdateScheduleState(doctorSchedule, StateScheduleEnum.Scheduled);
            return this;
        }


        public Doctor SchedulePatientAppointmentUpdateState(DoctorSchedulePatient appointment, StateScheduleEnum state)
        {
            appointment.SetState(state);
            return this;
        }

    }
}

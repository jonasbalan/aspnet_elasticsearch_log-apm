using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Npgsql;
using Polly;
using Polly.Retry;
using SamplesApi3.Model;

namespace SamplesApi3.Infrastructure
{
    public class DoctorScheduleDbContextSeed
    {

        public async Task SeedAsync(DoctorScheduleDbContext context, IWebHostEnvironment env, 
            ILogger<DoctorScheduleDbContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(DoctorScheduleDbContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!context.Doctor.Any())
                {
                    var doctors = CreateDoctors().ToList();

                    await context.Doctor.AddRangeAsync(doctors);
                    await context.SaveChangesAsync();
                }               
            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<DoctorScheduleDbContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<NpgsqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Error seeding database (attempt {retry} of {retries})", prefix, retry, retries);
                    }
                );
        }

        private IEnumerable<Doctor> CreateDoctors()
        {
            yield return CreateDoctorJoao();

            yield return CreateDoctorMaria();
        }        


        private Doctor CreateDoctorJoao()
        {
            var doctor = new Doctor()
            {
                Id = Guid.Parse("3fbe360e-2c5c-4a12-b9fb-4cb3137bc8ea"),
                Name = "João ABC",
                Register = "ABC-1234-zxc"
            };

            doctor.AddAddress("Rua XV", "99", "Centro", "Capivari", "SP", "13360-000", "Sala 02", "Brasil", "Clinia ABC");
            doctor.AddAddress("Av Pio XII", "1289", "Centro", "Capivari", "SP", "13360-000", "", "Brasil", "Clinia 123");

            doctor.AddSpecialties("Clinico Geral");
            doctor.AddSpecialties("Ortopedista");

            LoadSchedule(new DateTime(2023,11,08), 06,  doctor);

            LoadPatienties(doctor, new Patient()
            {
                Id = Guid.Parse("de3c79ac-7e70-4196-8eba-6d80790c51ee"),
                Name = "James Silva",
                BornDate = new DateTime(1969, 8, 26)

            });

            return doctor;
        }

        private Doctor CreateDoctorMaria()
        {
            var doctor = new Doctor()
            {
                Id = Guid.Parse("632bfda0-9f3c-49ce-8291-96144551339f"),
                Name = "Maria ZYB",
                 Register = "ABC-1234-zxc"
            };   

            doctor.AddAddress("Rua XV", "99", "Centro", "Capivari", "SP", "13360-000", "Sala 02", "Brasil", "Clinia ABC" );
            doctor.AddAddress("Av Pio XII", "1289", "Centro", "Capivari", "SP", "13360-000", "", "Brasil", "Clinia 123");

            doctor.AddSpecialties("Clinico Geral");
            doctor.AddSpecialties("Ortopedista");

            LoadSchedule(new DateTime(2023, 11, 08), 06, doctor);

            LoadPatienties(doctor, new Patient()
            {
                Id = Guid.Parse("8dd44cf5-cc0e-497b-b75a-a5727d027c0f"),
                Name = "Nelson Macley",
                BornDate = new DateTime(1999, 2, 26)

            });

            return doctor;           
        }

        private static void LoadSchedule(DateTime baseFrom, int days, Doctor doctor)
        {
            //foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            for (DateTime datetime = baseFrom; datetime < baseFrom.AddDays(days); datetime = datetime.AddDays(1))          
            {

                if (datetime.DayOfWeek != DayOfWeek.Saturday && datetime.DayOfWeek != DayOfWeek.Monday)
                {
                    for (TimeOnly time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)); time < TimeOnly.FromTimeSpan(TimeSpan.FromHours(18)); time = time.Add(TimeSpan.FromMinutes(30)))
                    {
                        string specialty = Random(doctor.Specialties).Specialty;
                        var address = Random(doctor.Adresses);

                        if (time < TimeOnly.FromTimeSpan(TimeSpan.FromHours(12)) || time > TimeOnly.FromTimeSpan(TimeSpan.FromHours(14)))                                                  
                            doctor.AddSchedule(datetime.Date + time.ToTimeSpan(), 30, Model.Enum.StateScheduleEnum.Free, address, specialty);                        
                        else                        
                            doctor.AddSchedule(datetime.Date + time.ToTimeSpan(), 30, Model.Enum.StateScheduleEnum.Blocked, address, specialty);
                        
                    }
                }                
            }
        }

        private static void LoadPatienties(Doctor doctor, Patient patient)
        {
            var schedule = Random(doctor.Schedules.Where(x=>x.State == Model.Enum.StateScheduleEnum.Free));
            var specialty = Random(schedule.DoctorSpecialties);
            doctor.SchedulePatientAppointment(patient, schedule, specialty);
        }


        private static T Random<T>(IEnumerable<T> list)
        {
            if (list.Any())
            {
                var r = new Random();
                int index = r.Next(0, list.Count() - 1);
                return list.ElementAt(index);
            }
            return default(T);
        }

       
    }
}

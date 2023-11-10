using Microsoft.EntityFrameworkCore;
using SamplesApi3.Model;
using SamplesApi3.Infrastructure.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore.Design;
using System.Runtime.CompilerServices;

namespace SamplesApi3.Infrastructure
{

    

    public class DoctorScheduleDbContext : DbContext
    {

        public const string connectionString = "Server=postgresql;Port=5432;Database=sample-api3-db;User Id=sample-api3-db-user;Password=sample-api3-db-user-pass;";
        public DoctorScheduleDbContext(DbContextOptions<DoctorScheduleDbContext> options) : base(options)
        {
        }

        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<DoctorSpecialty> DoctorSpecialty { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedule { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<DoctorSchedulePatient> DoctorSchedulePatient { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new DoctorEntityTypeConfiguration());
            builder.ApplyConfiguration(new DoctorSpecialtyEntityTypeConfiguration());
            builder.ApplyConfiguration(new DoctorScheduleEntityTypeConfiguration());
            builder.ApplyConfiguration(new PatientEntityTypeConfiguration());
            builder.ApplyConfiguration(new DoctorAddressEntityTypeConfiguration());
            builder.ApplyConfiguration(new DoctorSchedulePatientEntityTypeConfiguration());
        }
    }

    public class DoctorScheduleDbContextDesignFactory : IDesignTimeDbContextFactory<DoctorScheduleDbContext>
    {
        public DoctorScheduleDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DoctorScheduleDbContext>()
                .UseNpgsql(DoctorScheduleDbContext.connectionString);

            return new DoctorScheduleDbContext(optionsBuilder.Options);
        }
    }


    public static class MyModuleInitializer
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }

}

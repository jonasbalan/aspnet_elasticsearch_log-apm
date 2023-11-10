using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SamplesApi3.Model;

namespace SamplesApi3.Infrastructure.EntitiesConfigurations
{
    internal class DoctorSchedulePatientEntityTypeConfiguration : IEntityTypeConfiguration<DoctorSchedulePatient>
    {
        public void Configure(EntityTypeBuilder<DoctorSchedulePatient> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.DoctorSchedule);
            builder.HasOne(x => x.Specialty);
            builder.HasOne(x => x.Patient);
        }
    }
}
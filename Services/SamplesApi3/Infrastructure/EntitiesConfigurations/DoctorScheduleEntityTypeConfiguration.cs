using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SamplesApi3.Model;

namespace SamplesApi3.Infrastructure.EntitiesConfigurations
{
    internal class DoctorScheduleEntityTypeConfiguration : IEntityTypeConfiguration<DoctorSchedule>
    {
        public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.DoctorSpecialties);
            builder.HasOne(x => x.Address);
            builder.HasOne(x => x.Doctor).WithMany(x => x.Schedules);

            
        }
    }
}
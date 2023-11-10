using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SamplesApi3.Model;

namespace SamplesApi3.Infrastructure.EntitiesConfigurations
{
    internal class DoctorEntityTypeConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.Adresses).WithOne(x => x.Doctor);
            builder.HasMany(x => x.Specialties).WithOne(x => x.Doctor);
            builder.HasMany(x => x.Schedules).WithOne(x => x.Doctor);            
        }
    }
}
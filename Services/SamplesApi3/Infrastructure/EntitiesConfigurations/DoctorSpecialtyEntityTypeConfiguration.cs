using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SamplesApi3.Model;

namespace SamplesApi3.Infrastructure.EntitiesConfigurations
{
    internal class DoctorSpecialtyEntityTypeConfiguration : IEntityTypeConfiguration<DoctorSpecialty>
    {
        public void Configure(EntityTypeBuilder<DoctorSpecialty> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Doctor).WithMany(x => x.Specialties);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SamplesApi3.Model;

namespace SamplesApi3.Infrastructure.EntitiesConfigurations
{
    internal class DoctorAddressEntityTypeConfiguration : IEntityTypeConfiguration<DoctorAddress>
    {
        public void Configure(EntityTypeBuilder<DoctorAddress> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Doctor).WithMany(x => x.Adresses);
        }
    }
}
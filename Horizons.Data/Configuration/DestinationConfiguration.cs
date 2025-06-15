
namespace Horizons.Data.Configuration
{
    using Horizons.Data.Models;
    using static Horizons.GCommon.ValidationConstatnts.Destination;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class DestinationConfiguration : IEntityTypeConfiguration<Destination>
    {
        public void Configure(EntityTypeBuilder<Destination> entity)
        {
            entity
                .HasKey(d => d.Id);

            entity
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(d => d.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength);

            entity
                .Property(d => d.ImageUrl)
                .IsRequired(false);

            entity
                .Property(d => d.PublisherId)
                .IsRequired();

            entity
                .Property(d => d.IsDeleted)
                .HasDefaultValue(false);

            //take only activ Destination
            entity.HasQueryFilter(d => d.IsDeleted == false);

            entity
                .HasOne(d => d.Publisher)
                .WithMany()
                .HasForeignKey(d => d.PublisherId);

            entity
                .HasOne(d => d.Terrain)
                .WithMany(t => t.Destinations)
                .HasForeignKey(d => d.TerrainId);
        }
    }
}

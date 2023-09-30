using Domain.CompanySpace;
using Domain.CompanySpace.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SpaceConfiguration : IEntityTypeConfiguration<CompanySpace>
{
    public void Configure(EntityTypeBuilder<CompanySpace> builder)
    {
        builder.ToTable("Space");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => CompanySpaceId.Create(value));

        builder.Property(x => x.Name);

        builder.OwnsMany(x => x.Members, mi =>
        {
            mi.WithOwner().HasForeignKey("SpaceId");
            mi.HasKey("SpaceId", "Value");

            mi.Property(x => x.Value).ValueGeneratedNever();
        });
        builder.OwnsMany(x => x.PostIds, pi =>
        {
            pi.WithOwner().HasForeignKey("SpaceId");
            pi.HasKey("SpaceId", "Value");

            pi.Property(x => x.Value).ValueGeneratedNever();
        });

        builder.Navigation(x => x.Members)
            .HasField("_members")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(x => x.PostIds)
            .HasField("_postIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

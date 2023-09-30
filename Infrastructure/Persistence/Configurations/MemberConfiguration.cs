using Domain.CompanySpace.ValueObjects;
using Domain.Member;
using Domain.Member.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => MemberId.Create(value));

        builder.Property(x => x.Name);
        builder.Property(x => x.Email);
        builder.Property(x => x.Password);
        builder.Property(x => x.CompanySpaceId)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => CompanySpaceId.Create(value));

        builder.OwnsMany(x => x.PostIds, pi =>
        {
            pi.WithOwner().HasForeignKey("MemberId");
            pi.HasKey("MemberId", "Value");
            pi.Property(x => x.Value).ValueGeneratedNever();
        });

        builder.Navigation(x => x.PostIds)
            .HasField("_postIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

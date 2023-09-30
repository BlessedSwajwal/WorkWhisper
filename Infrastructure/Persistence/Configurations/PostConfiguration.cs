using Domain.CompanySpace.ValueObjects;
using Domain.Member.ValueObjects;
using Domain.Post;
using Domain.Post.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("PostId")
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => PostId.Create(value));

        builder.Property(x => x.OwnerId)
            .HasConversion(id => id.Value, value => MemberId.Create(value));

        builder.Property(x => x.SpaceId)
            .HasConversion(id => id.Value, value => CompanySpaceId.Create(value));

        builder.OwnsMany(x => x.Comments, cb =>
        {
            cb.WithOwner().HasForeignKey("PostId");
            cb.HasKey("Id", "PostId");

            cb.Property(x => x.CommentorId)
                .HasConversion(id => id.Value, value => MemberId.Create(value));

            cb.Property(x => x.Id)
                .HasColumnName("CommentId")
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => CommentId.Create(value));

            cb.OwnsMany(x => x.UpvotingMemberIds, ub => {
                ub.WithOwner().HasForeignKey("CommentId", "PostId");
                ub.HasKey("Value", "CommentId", "PostId");
                ub.Property(x => x.Value).ValueGeneratedNever();
            });
            cb.Navigation(x => x.UpvotingMemberIds)
                .HasField("_upvotingMemberIds")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.OwnsMany(x => x.UpvotingMemberIds, ub => {
            ub.WithOwner().HasForeignKey("PostId");
            ub.HasKey("Value", "PostId");
            ub.Property(x => x.Value).ValueGeneratedNever();
        });

        builder.Navigation(x => x.UpvotingMemberIds)
                .HasField("_upvotingMemberIds")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(x => x.Comments)
            .HasField("_comments")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

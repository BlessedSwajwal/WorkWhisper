using Domain.CompanySpace;
using Domain.Member;
using Domain.Post;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence;

public class WorkWhisperDbContext : DbContext
{
    public WorkWhisperDbContext(DbContextOptions options) : base(options)
    {}
    public DbSet<CompanySpace> CompanySpaces { get; set;}
    public DbSet<Member> Members { get; set;}
    public DbSet<Post> Posts { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkWhisperDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

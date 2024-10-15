using System.Reflection;
using PetCareClub.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace PetCareClub.Infra.Data.EF;
public class PetCareClubDbContext 
    : DbContext
{
    public DbSet<User> Users => Set<User>();

    public PetCareClubDbContext(
        DbContextOptions<PetCareClubDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
    }
}


using System.Net.Mime;
using AdoptAPet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdoptAPet.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Advertisement> Advertisements { get; set; }
    public DbSet<Application> Applications { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        List<IdentityRole> roles =
        [
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },

            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            },

            new IdentityRole
            {
                Name = "Rescue Team",
                NormalizedName = "RESCUE TEAM"
            }
        ];
        modelBuilder.Entity<IdentityRole>().HasData(roles);

        // Configure one-to-many relationship between Pet and Advertisement
        modelBuilder.Entity<Pet>()
            .HasMany(p => p.Advertisements)
            .WithOne(ad => ad.Pet)
            .HasForeignKey(ad => ad.PetId);

        // Configure one-to-many relationship between User and Application
        modelBuilder.Entity<User>()
            .HasMany(u => u.Applications)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .IsRequired();

        // Configure one-to-many relationship between Advertisement and Application
        modelBuilder.Entity<Advertisement>()
            .HasMany(ad => ad.Applications)
            .WithOne(a => a.Advertisement)
            .HasForeignKey(a => a.AdvertisementId)
            .IsRequired();
    }
}
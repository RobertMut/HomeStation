using HomeStation.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeStation.Infrastructure.Persistence;

public class AirDbContext : DbContext
{
    public AirDbContext(DbContextOptions options) : base(options) { }
    
    /// <summary>
    /// Climate readings
    /// </summary>
    public DbSet<Climate> Climate { get; set; }
    
    /// <summary>
    /// Air Quality readings
    /// </summary>
    public DbSet<Quality> AirQuality { get; set; }

    /// <summary>
    /// Devices
    /// </summary>
    public DbSet<Device> Devices { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Climate>().OwnsOne(x => x.Reading);
        modelBuilder.Entity<Climate>().HasKey(x => x.Id);
        modelBuilder.Entity<Climate>().Property(x => x.Id).ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Quality>().OwnsOne(x => x.Reading);
        modelBuilder.Entity<Quality>().HasKey(x => x.Id);
        modelBuilder.Entity<Quality>().Property(x => x.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Device>().HasKey(x => x.Id);
        modelBuilder.Entity<Device>().Property(x => x.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Device>().HasMany(x => x.Climate)
            .WithOne(x => x.Device)
            .HasForeignKey(x => x.DeviceId);
        modelBuilder.Entity<Device>().HasMany(x => x.AirQuality)
            .WithOne(x => x.Device)
            .HasForeignKey(x => x.DeviceId);
        
        base.OnModelCreating(modelBuilder);
    }
}
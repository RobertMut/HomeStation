using Microsoft.EntityFrameworkCore;

namespace HomeStation.Infrastructure.Persistence;

public class AirDbContextFactory : DesignTimeDbContextFactoryBase<AirDbContext>
{
    protected override AirDbContext CreateNewInstance(DbContextOptions<AirDbContext> options)
    {
        return new AirDbContext(options);
    } 
}
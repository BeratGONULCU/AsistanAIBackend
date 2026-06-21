using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GeminiAsistanBackend.Infrastructure;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // PostgreSQL için verdiğiniz bağlantı cümlesini buraya ekliyoruz
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=GeminiAsistanBackendDb;Username=postgres;Password=123456");

        return new AppDbContext(optionsBuilder.Options);
    }
}

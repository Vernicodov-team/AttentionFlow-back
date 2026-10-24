using Microsoft.EntityFrameworkCore;

namespace DB;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
        Database.EnsureCreated();
    }
}
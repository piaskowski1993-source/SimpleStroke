using Microsoft.EntityFrameworkCore;
 
namespace SS.Api.Data;

public class SSDbContext : DbContext
{
    public DbSet<User> Users { get; set;}
    public SSDbContext(DbContextOptions<SSDbContext> options) : base(options)
    {}
}
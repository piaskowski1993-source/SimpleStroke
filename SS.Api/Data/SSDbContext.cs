using Microsoft.EntityFrameworkCore;
 
namespace SS.Api.Data;

public class SSDbContext : DbContext
{
    public DbSet<User> Users { get; set;}
    public SSDbContext(DbContextOptions<SSDbContext> options) : base(options)
    {}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().Property(u => u.Shape);    
    }
}
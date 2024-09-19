using Microsoft.EntityFrameworkCore;
using ZYTreeHole_Models.Entity;

namespace ZYTreeHole_Models;

public class MyDbContext: DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options): base(options){}
    public DbSet<ZyUsers> users { get; set; }
    public DbSet<ZyComments> comments { get; set; }
    public DbSet<LikeRecords> likeRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
    
}
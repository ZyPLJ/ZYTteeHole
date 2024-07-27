using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ZYTreeHole_Models;

internal class MyDbContextDesignFac : IDesignTimeDbContextFactory<MyDbContext>
{
    public MyDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<MyDbContext> builder = new DbContextOptionsBuilder<MyDbContext>();
        string connStr = @"Server=DESKTOP-U64RN7B\SQLSERVER;Database=TreeHole;User Id=Sa;Password=zyplj1314999;Encrypt=True;TrustServerCertificate=True;";
        builder.UseSqlServer(connStr);
        MyDbContext ctx = new MyDbContext(builder.Options);
        return ctx;
    }
}
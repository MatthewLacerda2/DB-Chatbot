using Microsoft.EntityFrameworkCore;
using DB_Chatbot.Server.Models;

namespace Server.Data;

public class ServerContext : DbContext
{

    public DbSet<Person> People { get; set; } = default!;
    public DbSet<Item> Items { get; set; } = default!;

    public ServerContext(DbContextOptions<ServerContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Person>().ToTable("People");
        builder.Entity<Item>().ToTable("Items");

        //FlowSeeder flowSeeder = new FlowSeeder(builder, 777);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlite("Data Source=:memory:");
    }

}
using Microsoft.EntityFrameworkCore;
using DB_Chatbot.Server.Models;
using Microsoft.Data.Sqlite;
using DB_Chatbot.Server.Utils;

namespace Server.Data;

public class ServerContext : DbContext
{

    public DbSet<Person> People { get; set; } = default!;
    public DbSet<Item> Items { get; set; } = default!;

    public ServerContext(DbContextOptions<ServerContext> options)
        : base(options)
    {
        Database.OpenConnection();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Person>().ToTable("People");
        builder.Entity<Item>().ToTable("Items");

        Seeder seeder = new Seeder();

        builder.Entity<Person>().HasData(seeder.GetPeople(50));
        builder.Entity<Item>().HasData(seeder.GetItems(50));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlite("Data Source=:memory:");
    }

}
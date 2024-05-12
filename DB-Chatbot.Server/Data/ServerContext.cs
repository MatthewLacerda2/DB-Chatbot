using Microsoft.EntityFrameworkCore;
using DB_Chatbot.Server.Models;

namespace Server.Data;

public class ServerContext : DbContext
{

    public DbSet<Pessoa> Pessoas { get; set; } = default!;
    public DbSet<Item> Items { get; set; } = default!;

    public ServerContext(DbContextOptions<ServerContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

}
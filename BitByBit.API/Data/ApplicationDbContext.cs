using Microsoft.EntityFrameworkCore;
using BitByBit.API.Models;

namespace BitByBit.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<PlayerStats> PlayerStats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Inventory)
                .WithOne(i => i.Player)
                .HasForeignKey<Inventory>(i => i.PlayerId);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Stats)
                .WithOne(s => s.Player)
                .HasForeignKey<PlayerStats>(s => s.PlayerId);

            modelBuilder.Entity<InventoryItem>()
                .HasOne(ii => ii.Inventory)
                .WithMany(i => i.Items)
                .HasForeignKey(ii => ii.InventoryId);

            modelBuilder.Entity<InventoryItem>()
                .HasOne(ii => ii.Item)
                .WithMany()
                .HasForeignKey(ii => ii.ItemId);

            // Configure Item entity
            modelBuilder.Entity<Item>()
                .HasMany(i => i.InventoryItems)
                .WithOne(ii => ii.Item)
                .HasForeignKey(ii => ii.ItemId);
        }
    }
} 
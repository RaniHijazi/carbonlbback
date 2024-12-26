using Microsoft.EntityFrameworkCore;
using StoreProject.Models;

namespace StoreProject.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Items)
                .WithOne(i => i.Category)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);


        }

    }
   
}

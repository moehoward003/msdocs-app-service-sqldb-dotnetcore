using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace DotNetCoreSqlDb.Models
{
    public interface IDatabaseContext
    {
        DbSet<Todo> Todo { get; set; }
     }

    public partial class MyDatabaseContext : Microsoft.EntityFrameworkCore.DbContext, IDatabaseContext
    {
        public MyDatabaseContext(DbContextOptions<MyDatabaseContext> options) : base(options)
       // public MyDatabaseContext() : base()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Efmigrationshistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

                entity.ToTable("__efmigrationshistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);
                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        public DbSet<Efmigrationshistory> Efmigrationshistories { get; set; } = null!;
        public virtual DbSet<Todo> Todo { get; set; } = null!;

    }

}

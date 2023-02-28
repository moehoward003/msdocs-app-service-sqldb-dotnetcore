using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreSqlDb.Models
{
    public partial class MyDatabaseContext : DbContext
    {
        public MyDatabaseContext (DbContextOptions<MyDatabaseContext> options)  : base(options)
        {

        }

        public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseMySQL("Server=msdocs-core-mysql-001-server.mysql.database.azure.com;UserID = mysqllogin;Password=!QAZxsw2;Database=MyDatabase;"
        //        );


        //optionsBuilder.UseMySQL("Server=msdocs-core-mysql-001-server.mysql.database.azure.com;UserID = mysqllogin;Password=!QAZxsw2;Database=MyDatabase;");
            //optionsBuilder.UseCosmos("AccountEndpoint=https://msdocs-core-sql-002-server.documents.azure.com:443/;AccountKey=JPFhH1gcByFLhT2PZMujYe493axPV0psUYkvVV0KwAjyzdEBFe2SprO0WDME4InEj90Ix6mRJXREACDbNDBqCA==", "msdocs-core-sql-002-database");
            //optionsBuilder.UseCosmos(Configuration.GetConnectionString("MyDbConnection"), "MyDatabase");
            //optionsBuilder.UseCosmos("https://msdocs-core-sql-002-server.documents.azure.com",
            //   "JPFhH1gcByFLhT2PZMujYe493axPV0psUYkvVV0KwAjyzdEBFe2SprO0WDME4InEj90Ix6mRJXREACDbNDBqCA==",
            //    "msdocs-core-sql-002-database");

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

        public DbSet<DotNetCoreSqlDb.Models.Todo> Todo { get; set; }
    }
}

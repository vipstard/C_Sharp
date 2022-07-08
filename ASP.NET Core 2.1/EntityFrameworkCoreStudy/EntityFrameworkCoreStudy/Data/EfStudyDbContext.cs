using EntityFrameworkCoreStudy.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCoreStudy.Data
{
    public class EfStudyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // UseSqlServer를 사용하기위해 EFCore.SqlServer를 설치해야한다?!
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AspnetCoreDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //EF Fluent API 
            modelBuilder.Entity<User>().ToTable("s_users");

            modelBuilder.Entity<User>()
                .Property(u => u.UserName)
                .HasColumnName("s_userName");
                

        }
    }
}

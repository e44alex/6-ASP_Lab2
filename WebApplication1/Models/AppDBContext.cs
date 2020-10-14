using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class AppDBContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects{ get; set; }
        public DbSet<StudentSubjectAttendance> Attendances { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasIndex(a => a.Id).IsUnique();

            modelBuilder.Entity<Subject>().HasIndex(a => a.Id).IsUnique();

            modelBuilder.Entity<StudentSubjectAttendance>().HasIndex(a => a.Id).IsUnique();
        }
    }
}
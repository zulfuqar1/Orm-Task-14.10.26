using Microsoft.EntityFrameworkCore;
using Orm_Task_14._10._26.Models;
using UniversityManagementSystem.Configurations;

namespace UniversityManagementSystem.Data;

public class UniversityDbContext : DbContext
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer( @"Server=ZULFUQAR;Database=UniversityDb;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(UniversityDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
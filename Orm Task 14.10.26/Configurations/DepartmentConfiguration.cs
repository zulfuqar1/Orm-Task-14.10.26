using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orm_Task_14._10._26.Models;

namespace UniversityManagementSystem.Configurations;

public class DepartmentConfiguration
    : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("DepartmentId")
            .ValueGeneratedOnAdd();

        builder.Property(d => d.Name)
            .HasColumnName("DepartmentName")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.HasMany(d => d.Teachers)
            .WithOne(t => t.Department)
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Courses)
            .WithOne(c => c.Department)
            .HasForeignKey(c => c.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
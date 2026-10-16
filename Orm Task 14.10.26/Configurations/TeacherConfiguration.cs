using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orm_Task_14._10._26.Models;

namespace UniversityManagementSystem.Configurations;

public class TeacherConfiguration
    : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("Teachers");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.FullName)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(t => t.Email)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(t => t.Email)
            .IsUnique();

        builder.HasOne(t => t.Department)
            .WithMany(d => d.Teachers)
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
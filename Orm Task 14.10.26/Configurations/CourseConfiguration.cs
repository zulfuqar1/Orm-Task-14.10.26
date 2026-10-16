using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orm_Task_14._10._26.Models;

namespace UniversityManagementSystem.Configurations;

public class CourseConfiguration
    : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses", table =>
        {
            table.HasCheckConstraint(
                "CK_Courses_Credit",
                "[Credit] BETWEEN 1 AND 10");
        });

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(c => c.Credit)
            .IsRequired();

        builder.HasOne(c => c.Department)
            .WithMany(d => d.Courses)
            .HasForeignKey(c => c.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Teacher)
            .WithMany(t => t.Courses)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Enrollments)
            .WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
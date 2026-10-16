using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orm_Task_14._10._26.Models;

namespace UniversityManagementSystem.Configurations;

public class EnrollmentConfiguration
    : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollments", table =>
        {
            table.HasCheckConstraint(
                "CK_Enrollments_Grade",
                "[Grade] IS NULL OR [Grade] BETWEEN 0 AND 100");
        });

        builder.HasKey(e => e.Id);

        builder.Property(e => e.EnrollmentDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(e => e.Grade)
            .HasColumnType("decimal(5,2)")
            .IsRequired(false);

        builder.HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.StudentId, e.CourseId })
            .IsUnique();
    }
}
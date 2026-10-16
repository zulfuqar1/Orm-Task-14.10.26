using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orm_Task_14._10._26.Models;

namespace UniversityManagementSystem.Configurations;

public class StudentConfiguration
    : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.FullName)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(s => s.Email)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(s => s.Email)
            .IsUnique();

        builder.Property(s => s.BirthDate)
            .HasColumnType("date")
            .IsRequired();

        builder.HasMany(s => s.Enrollments)
            .WithOne(e => e.Student)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
using Microsoft.EntityFrameworkCore;
using Orm_Task_14._10._26.Models;


namespace UniversityManagementSystem.Data;

public static class DbInitializer
{
    public static void Initialize(UniversityDbContext context)
    {
        context.Database.Migrate();

        if (context.Departments.Any())
            return;

        var department = new Department
        {
            Name = "Computer Science",
            Description = "IT və proqramlaşdırma şöbəsi"
        };

        var teacher = new Teacher
        {
            FullName = "Ali Mammadov",
            Email = "ali@university.az",
            Department = department
        };

        var course = new Course
        {
            Name = "C# Programming",
            Credit = 5,
            Department = department,
            Teacher = teacher
        };

        var student = new Student
        {
            FullName = "Aysel Aliyeva",
            Email = "aysel@university.az",
            BirthDate = new DateTime(2005, 5, 10)
        };

        var enrollment = new Enrollment
        {
            Student = student,
            Course = course,
            EnrollmentDate = DateTime.Today,
            Grade = null
        };

        context.Enrollments.Add(enrollment);
        context.SaveChanges();
    }
}
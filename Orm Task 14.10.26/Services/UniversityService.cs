
using Microsoft.EntityFrameworkCore;
using Orm_Task_14._10._26.Models;
using UniversityManagementSystem.Data;

namespace UniversityManagementSystem.Services;

public class UniversityService

//umumi service classi yaradilib, butun methodlar burda yazilib, her bir methodda lazim olan entityler yoxlanilir ve sonra database-e elave olunur.
//layihe icinde istifade edilmirler 
//ilk versiyada butun Crud emeliyatlari burda sitifade olunurdu daha sonradan her biri ayir servisde yazdim.
{
    public static void AddDepartment()
    {
        using var db = new UniversityDbContext();

        Console.Write("Department name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Description: ");
        string? description = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name is required.");
            return;
        }

        db.Departments.Add(new Department
        {
            Name = name,
            Description = description
        });

        db.SaveChanges();
        Console.WriteLine("Department added.");
    }
    public static void AddTeacher()
    {
        using var db = new UniversityDbContext();

        Console.Write("Full name: ");
        string fullName = Console.ReadLine() ?? "";

        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Department ID: ");
        if (!int.TryParse(Console.ReadLine(), out int departmentId))
            return;

        if (!db.Departments.Any(d => d.Id == departmentId))
        {
            Console.WriteLine("Department not found.");
            return;
        }

        db.Teachers.Add(new Teacher
        {
            FullName = fullName,
            Email = email,
            DepartmentId = departmentId
        });

        db.SaveChanges();
        Console.WriteLine("Teacher added.");
    }
    public static void AddCourse()
    {
        using var db = new UniversityDbContext();

        Console.Write("Course name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Credit (1-10): ");
        if (!int.TryParse(Console.ReadLine(), out int credit)
            || credit < 1 || credit > 10)
        {
            Console.WriteLine("Invalid credit.");
            return;
        }

        Console.Write("Department ID: ");
        if (!int.TryParse(Console.ReadLine(), out int departmentId))
            return;

        Console.Write("Teacher ID: ");
        if (!int.TryParse(Console.ReadLine(), out int teacherId))
            return;

        if (!db.Departments.Any(d => d.Id == departmentId) ||
            !db.Teachers.Any(t => t.Id == teacherId &&
                                  t.DepartmentId == departmentId))
        {
            Console.WriteLine(
                "Department or its teacher was not found.");
            return;
        }

        db.Courses.Add(new Course
        {
            Name = name,
            Credit = credit,
            DepartmentId = departmentId,
            TeacherId = teacherId
        });

        db.SaveChanges();
        Console.WriteLine("Course added.");
    }
    public static void AddStudent()
    {
        using var db = new UniversityDbContext();

        Console.Write("Full name: ");
        string fullName = Console.ReadLine() ?? "";

        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Birth date (yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime birthDate))
        {
            Console.WriteLine("Invalid date.");
            return;
        }

        db.Students.Add(new Student
        {
            FullName = fullName,
            Email = email,
            BirthDate = birthDate
        });

        db.SaveChanges();
        Console.WriteLine("Student added.");
    }
    public static void EnrollStudent()
    {
        using var db = new UniversityDbContext();

        Console.Write("Student ID: ");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
            return;

        Console.Write("Course ID: ");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
            return;

        if (!db.Students.Any(s => s.Id == studentId) ||
            !db.Courses.Any(c => c.Id == courseId))
        {
            Console.WriteLine("Student or course not found.");
            return;
        }

        if (db.Enrollments.Any(e =>
                e.StudentId == studentId && e.CourseId == courseId))
        {
            Console.WriteLine("Student is already enrolled.");
            return;
        }

        db.Enrollments.Add(new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrollmentDate = DateTime.Today,
            Grade = null
        });

        db.SaveChanges();
        Console.WriteLine("Student enrolled.");
    }
    public static void ShowDepartmentsWithTeachers()
    {
        using var db = new UniversityDbContext();

        var departments = db.Departments
            .Include(d => d.Teachers)
            .ToList();

        foreach (var d in departments)
        {
            Console.WriteLine($"\nDepartment: {d.Name}");

            foreach (var t in d.Teachers)
                Console.WriteLine($"  {t.FullName} | {t.Email}");
        }
    }
    public static void ShowDepartmentsWithCoursesAndTeachers()
    {
        using var db = new UniversityDbContext();

        var departments = db.Departments
            .Include(d => d.Courses)
                .ThenInclude(c => c.Teacher)
            .ToList();

        foreach (var d in departments)
        {
            Console.WriteLine($"\nDepartment: {d.Name}");

            foreach (var c in d.Courses)
                Console.WriteLine(
                    $"  {c.Name} | Credit: {c.Credit} | " +
                    $"Teacher: {c.Teacher.FullName}");
        }
    }
    public static void ShowStudentsWithCourses()
    {
        using var db = new UniversityDbContext();

        var students = db.Students
            .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
            .ToList();

        foreach (var s in students)
        {
            Console.WriteLine($"\nStudent: {s.FullName}");

            foreach (var e in s.Enrollments)
                Console.WriteLine(
                    $"  Course: {e.Course.Name} | " +
                    $"Date: {e.EnrollmentDate:yyyy-MM-dd} | " +
                    $"Grade: {e.Grade?.ToString() ?? "Not graded"}");
        }
    }
    public static void ShowCoursesWithStudents()
    {
        using var db = new UniversityDbContext();

        var courses = db.Courses
            .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
            .ToList();

        foreach (var c in courses)
        {
            Console.WriteLine($"\nCourse: {c.Name}");

            foreach (var e in c.Enrollments)
                Console.WriteLine(
                    $"  {e.Student.FullName} | " +
                    $"Grade: {e.Grade?.ToString() ?? "Not graded"}");
        }
    }
    public static void UpdateStudent()
    {
        using var db = new UniversityDbContext();

        Console.Write("Student ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
            return;

        var student = db.Students.Find(id);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        Console.Write("New full name: ");
        string name = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(name))
            return;

        student.FullName = name;
        db.SaveChanges();

        Console.WriteLine("Student updated.");
    }
    public static void DeleteStudent()
    {
        using var db = new UniversityDbContext();

        Console.Write("Student ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
            return;

        var student = db.Students.Find(id);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        db.Students.Remove(student);
        db.SaveChanges();

        Console.WriteLine(
            "Student and related enrollments deleted.");
    }
    public static void TestDeleteBehavior()
    {
        Console.WriteLine("1. Delete Department");
        Console.WriteLine("2. Delete Course");
        Console.WriteLine("3. Delete Student");
        Console.Write("Choose: ");

        string? choice = Console.ReadLine();

        Console.Write("Entity ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
            return;

        using var db = new UniversityDbContext();

        try
        {
            switch (choice)
            {
                case "1":
                    var department = db.Departments.Find(id);
                    if (department == null)
                    {
                        Console.WriteLine("Department not found.");
                        return;
                    }

                    db.Departments.Remove(department);
                    break;

                case "2":
                    var course = db.Courses.Find(id);
                    if (course == null)
                    {
                        Console.WriteLine("Course not found.");
                        return;
                    }

                    db.Courses.Remove(course);
                    break;

                case "3":
                    var student = db.Students.Find(id);
                    if (student == null)
                    {
                        Console.WriteLine("Student not found.");
                        return;
                    }

                    db.Students.Remove(student);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }

            db.SaveChanges();
            Console.WriteLine("Delete succeeded.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Delete failed: {ex.Message}");
        }
    }
    public static void ShowChangeTracker()
    {
        using var db = new UniversityDbContext();

        var student = db.Students.FirstOrDefault();

        if (student != null)
        {
            Console.WriteLine(
                $"Unchanged: {db.Entry(student).State}");

            student.FullName += " Test";
            db.ChangeTracker.DetectChanges();

            Console.WriteLine(
                $"Modified: {db.Entry(student).State}");
        }

        var detached = new Student
        {
            FullName = "Detached Student",
            Email = "detached@example.com",
            BirthDate = new DateTime(2000, 1, 1)
        };

        Console.WriteLine(
            $"Detached: {db.Entry(detached).State}");

        db.Students.Attach(detached);

        Console.WriteLine(
            $"After Attach: {db.Entry(detached).State}");

        db.Students.Add(new Student
        {
            FullName = "Added Student",
            Email = $"added_{Guid.NewGuid():N}@example.com",
            BirthDate = new DateTime(2000, 1, 1)
        });

        db.ChangeTracker.DetectChanges();

        Console.WriteLine("\nTracked entities:");

        foreach (var entry in db.ChangeTracker.Entries())
        {
            Console.WriteLine(
                $"{entry.Metadata.ClrType.Name}: {entry.State}");
        }
    }
}

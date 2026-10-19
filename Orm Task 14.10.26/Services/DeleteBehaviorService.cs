using Microsoft.EntityFrameworkCore;
using Orm_Task_14._10._26.Models;
using UniversityManagementSystem.Data;

namespace Orm_Task_14._10._26.Services;

public static class DeleteBehaviorService
{
    private const bool RollbackAfterTest = true;

    public static void Run()
    {
        Console.WriteLine("\n--- DeleteBehavior ssenariləri ---");
        Console.WriteLine("1. Department sil (Teacher və Course var) - Restrict");
        Console.WriteLine("2. Teacher sil (Course var) - Restrict");
        Console.WriteLine("3. Course sil (Enrollment var) - Cascade");
        Console.WriteLine("4. Student sil (Enrollment var) - Cascade");
        Console.Write("Choose: ");
        string? choice = Console.ReadLine();

        Console.Write("Entity ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        switch (choice)
        {
            case "1": DeleteDepartment(id); break;
            case "2": DeleteTeacher(id); break;
            case "3": DeleteCourse(id); break;
            case "4": DeleteStudent(id); break;
            default: Console.WriteLine("Invalid choice."); break;
        }
    }

    private static void DeleteDepartment(int id)
    {
        using var db = new UniversityDbContext();
        var department = db.Departments.Find(id);
        if (department == null) { Console.WriteLine("Department not found."); return; }

        void Print(string when) => Console.WriteLine(
            $"{when}: Department={db.Departments.Count(d => d.Id == id)}, " +
            $"Teacher={db.Teachers.Count(t => t.DepartmentId == id)}, " +
            $"Course={db.Courses.Count(c => c.DepartmentId == id)}");

        Console.WriteLine("Qayda: Department -> Teacher/Course = Restrict");
        Print("Əvvəl");
        Execute(db, () => db.Departments.Remove(department), () => Print("Sonra"));
    }

    private static void DeleteTeacher(int id)
    {
        using var db = new UniversityDbContext();
        var teacher = db.Teachers.Find(id);
        if (teacher == null) { Console.WriteLine("Teacher not found."); return; }

        void Print(string when) => Console.WriteLine(
            $"{when}: Teacher={db.Teachers.Count(t => t.Id == id)}, " +
            $"Course={db.Courses.Count(c => c.TeacherId == id)}");

        Console.WriteLine("Qayda: Teacher -> Course = Restrict");
        Print("Əvvəl");
        Execute(db, () => db.Teachers.Remove(teacher), () => Print("Sonra"));
    }

    private static void DeleteCourse(int id)
    {
        using var db = new UniversityDbContext();
        var course = db.Courses.Find(id);
        if (course == null) { Console.WriteLine("Course not found."); return; }

        void Print(string when) => Console.WriteLine(
            $"{when}: Course={db.Courses.Count(c => c.Id == id)}, " +
            $"Enrollment={db.Enrollments.Count(e => e.CourseId == id)}");

        Console.WriteLine("Qayda: Course -> Enrollment = Cascade");
        Print("Əvvəl");
        Execute(db, () => db.Courses.Remove(course), () => Print("Sonra"));
    }

    private static void DeleteStudent(int id)
    {
        using var db = new UniversityDbContext();
        var student = db.Students.Find(id);
        if (student == null) { Console.WriteLine("Student not found."); return; }

        void Print(string when) => Console.WriteLine(
            $"{when}: Student={db.Students.Count(s => s.Id == id)}, " +
            $"Enrollment={db.Enrollments.Count(e => e.StudentId == id)}");

        Console.WriteLine("Qayda: Student -> Enrollment = Cascade");
        Print("Əvvəl");
        Execute(db, () => db.Students.Remove(student), () => Print("Sonra"));
    }

    private static void Execute(UniversityDbContext db, Action remove, Action printAfter)
    {
        using var tx = db.Database.BeginTransaction();
        try
        {
            remove();
            int rows = db.SaveChanges();

            Console.WriteLine($"[UĞURLU] Silmə tamamlandı. Təsirlənən sətir sayı: {rows}");
            printAfter();

            if (RollbackAfterTest)
            {
                tx.Rollback();
                Console.WriteLine("(Test məqsədilə dəyişikliklər geri qaytarıldı.)");
            }
            else
            {
                tx.Commit();
            }
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("[UĞURSUZ] Silmə bloklandı (Restrict / FK məhdudiyyəti).");
            Console.WriteLine($"Səbəb: {ex.InnerException?.Message ?? ex.Message}");
            Console.WriteLine("Bazada heç bir dəyişiklik olmadı.");
            tx.Rollback();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[XƏTA] {ex.Message}");
            tx.Rollback();
        }
    }
}
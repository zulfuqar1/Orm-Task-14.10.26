using Microsoft.EntityFrameworkCore;
using Orm_Task_14._10._26.Models;
using UniversityManagementSystem.Data;

namespace Orm_Task_14._10._26.Services;

public static class EntityStateService
{
    public static void Run()
    {
        using var db = new UniversityDbContext();
        using var tx = db.Database.BeginTransaction();

        try
        {
            var added = Added(db);
            Modified(db);
            Unchanged(db);
            Detached(db);
            Deleted(db, added);
            ChangeTrackerDemo(db);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[XƏTA] {ex.Message}");
        }
        finally
        {
            tx.Rollback();
            Console.WriteLine("\n(Demo məlumatları bazaya yazılmadı, geri qaytarıldı.)");
        }
    }

    private static Student Added(UniversityDbContext db)
    {
        Console.WriteLine("\n=== A. Added ===");
        var student = new Student
        {
            FullName = "Demo Student",
            Email = $"demo_{Guid.NewGuid():N}@example.com",
            BirthDate = new DateTime(2005, 1, 1)
        };

        db.Students.Add(student);
        Console.WriteLine($"Add() sonrası: {db.Entry(student).State}");

        db.SaveChanges();
        Console.WriteLine($"SaveChanges() sonrası: {db.Entry(student).State}");
        return student;
    }

    private static void Modified(UniversityDbContext db)
    {
        Console.WriteLine("\n=== B. Modified ===");
        var student = db.Students.OrderBy(s => s.Id).First();

        Console.WriteLine($"Dəyişiklikdən əvvəl: {db.Entry(student).State}");
        student.FullName += " (updated)";
        db.ChangeTracker.DetectChanges();
        Console.WriteLine($"Dəyişiklikdən sonra: {db.Entry(student).State}");

        db.SaveChanges();
        Console.WriteLine($"SaveChanges() sonrası: {db.Entry(student).State}");
    }

    private static void Unchanged(UniversityDbContext db)
    {
        Console.WriteLine("\n=== D. Unchanged ===");
        var student = db.Students.OrderBy(s => s.Id).First();
        Console.WriteLine($"Bazadan alındı, dəyişiklik yoxdur: {db.Entry(student).State}");
    }

    private static void Detached(UniversityDbContext db)
    {
        Console.WriteLine("\n=== E. Detached ===");
        
        var student = new Student
        {
            Id = 99999,
            FullName = "Detached Student",
            Email = $"detached_{Guid.NewGuid():N}@example.com",
            BirthDate = new DateTime(2000, 1, 1)
        };

        Console.WriteLine($"Context-ə əlavə etməzdən əvvəl: {db.Entry(student).State}");
        db.Students.Attach(student);
        Console.WriteLine($"Attach() sonrası: {db.Entry(student).State}");

        db.Entry(student).State = EntityState.Detached; // SaveChanges-ə mane olmasın
    }

    private static void Deleted(UniversityDbContext db, Student student)
    {
        Console.WriteLine("\n=== C. Deleted ===");
        db.Students.Remove(student);
        Console.WriteLine($"Remove() sonrası: {db.Entry(student).State}");

        db.SaveChanges();
        Console.WriteLine($"SaveChanges() sonrası: {db.Entry(student).State}");
    }

    private static void ChangeTrackerDemo(UniversityDbContext db)
    {
        Console.WriteLine("\n=== F. ChangeTracker ===");
        db.ChangeTracker.Clear();

        var students = db.Students.OrderBy(s => s.Id).Take(2).ToList();
        foreach (var s in students)
            s.FullName += " *";                                   // Modified

        db.Students.Add(new Student                               // Added
        {
            FullName = "Tracker Student",
            Email = $"tracker_{Guid.NewGuid():N}@example.com",
            BirthDate = new DateTime(2001, 1, 1)
        });

        var course = db.Courses.FirstOrDefault();
        if (course != null)
            db.Entry(course).State = EntityState.Unchanged;       // Unchanged

        db.ChangeTracker.DetectChanges();

        Console.WriteLine("SaveChanges-dən əvvəl:");
        PrintEntries(db);

        db.SaveChanges();

        Console.WriteLine("\nSaveChanges-dən sonra:");
        PrintEntries(db);
    }

    private static void PrintEntries(UniversityDbContext db)
    {
        foreach (var entry in db.ChangeTracker.Entries())
            Console.WriteLine($"  {entry.Metadata.ClrType.Name,-12} -> {entry.State}");
    }
}
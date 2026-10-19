using Microsoft.EntityFrameworkCore;
using Orm_Task_14._10._26.Models;
using UniversityManagementSystem.Data;

namespace UniversityManagementSystem.Services;

public class StudentService
{
    private readonly UniversityDbContext _db;

    public StudentService(UniversityDbContext db)
    {
        _db = db;
    }

    public Student Add(string fullName, string email, DateTime birthDate)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Tələbənin adı boş ola bilməz.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email boş ola bilməz.");

        if (birthDate.Date > DateTime.Today)
            throw new ArgumentException("Doğum tarixi gələcək tarix ola bilməz.");

        Student student = new Student
        {
            FullName = fullName,
            Email = email,
            BirthDate = birthDate
        };

        _db.Students.Add(student);
        _db.SaveChanges();

        return student;
    }

    public List<Student> GetAll()
    {
        return _db.Students
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .ToList();
    }

    public List<Student> GetWithEnrollments()
    {
        return _db.Students
            .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .ToList();
    }

    public bool Update(int id, string fullName, string email)
    {
        Student? student = _db.Students.Find(id);

        if (student == null)
            return false;

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Ad boş ola bilməz.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email boş ola bilməz.");

        student.FullName = fullName;
        student.Email = email;

        _db.SaveChanges();

        return true;
    }

    public bool Delete(int id)
    {
        Student? student = _db.Students.Find(id);

        if (student == null)
            return false;

        _db.Students.Remove(student);
        _db.SaveChanges();

        return true;
    }

    public List<StudentReportRow> GetReport()
    {
        return _db.Enrollments
            .AsNoTracking()
            .Select(e => new StudentReportRow
            {
                Student = e.Student.FullName,
                Course = e.Course.Name,
                Date = e.EnrollmentDate,
                Grade = e.Grade
            })
            .OrderBy(r => r.Student)
            .ToList();
    }
}

public class StudentReportRow
{
    public string Student { get; set; } = string.Empty;
    public string Course { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal? Grade { get; set; }
}
using Orm_Task_14._10._26.Models;
using UniversityManagementSystem.Data;

namespace UniversityManagementSystem.Services;

public class EnrollmentService
{
    private readonly UniversityDbContext _db;

    public EnrollmentService(UniversityDbContext db)
    {
        _db = db;
    }

    public Enrollment Enroll(int studentId, int courseId)
    {
        if (!_db.Students.Any(s => s.Id == studentId))
            throw new ArgumentException("Tələbə tapılmadı.");

        if (!_db.Courses.Any(c => c.Id == courseId))
            throw new ArgumentException("Kurs tapılmadı.");

        if (_db.Enrollments.Any(e =>
                e.StudentId == studentId &&
                e.CourseId == courseId))
        {
            throw new InvalidOperationException(
                "Tələbə artıq bu kursa yazılıb.");
        }

        Enrollment enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrollmentDate = DateTime.Today,
            Grade = null
        };

        _db.Enrollments.Add(enrollment);
        _db.SaveChanges();

        return enrollment;
    }
}
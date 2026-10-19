using Microsoft.EntityFrameworkCore;
using Orm_Task_14._10._26.Models;
using UniversityManagementSystem.Data;

namespace UniversityManagementSystem.Services;

public class CourseService
{
    private readonly UniversityDbContext _db;

    public CourseService(UniversityDbContext db)
    {
        _db = db;
    }

    public Course Add(
        string name,
        int credit,
        int departmentId,
        int teacherId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Kurs adı boş ola bilməz.");

        if (credit < 1 || credit > 10)
            throw new ArgumentException("Credit 1-10 arasında olmalıdır.");

        if (!_db.Departments.Any(d => d.Id == departmentId))
            throw new ArgumentException("Department tapılmadı.");

        if (!_db.Teachers.Any(t =>
                t.Id == teacherId &&
                t.DepartmentId == departmentId))
        {
            throw new ArgumentException(
                "Müəllim tapılmadı və ya həmin department-ə aid deyil.");
        }

        Course course = new Course
        {
            Name = name,
            Credit = credit,
            DepartmentId = departmentId,
            TeacherId = teacherId
        };

        _db.Courses.Add(course);
        _db.SaveChanges();

        return course;
    }

    public List<Course> GetAll()
    {
        return _db.Courses
            .Include(c => c.Teacher)
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .ToList();
    }

    public List<Course> GetWithStudents()
    {
        return _db.Courses
            .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .ToList();
    }


    public Course UpdateCourse(
    int id,
    string name,
    int credit,
    int departmentId,
    int teacherId)
    {
        Course course = _db.Courses.Find(id)
            ?? throw new ArgumentException("Kurs tapılmadı.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Kurs adı boş ola bilməz.");

        if (credit < 1 || credit > 10)
            throw new ArgumentException("Credit 1-10 arasında olmalıdır.");

        if (!_db.Departments.Any(d => d.Id == departmentId))
            throw new ArgumentException("Department tapılmadı.");

        if (!_db.Teachers.Any(t =>
                t.Id == teacherId &&
                t.DepartmentId == departmentId))
        {
            throw new ArgumentException(
                "Müəllim tapılmadı və ya həmin department-ə aid deyil.");
        }

        course.Name = name;
        course.Credit = credit;
        course.DepartmentId = departmentId;
        course.TeacherId = teacherId;

        _db.SaveChanges();

        return course;
    }


}
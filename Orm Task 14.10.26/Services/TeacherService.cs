using Microsoft.EntityFrameworkCore;
using Orm_Task_14._10._26.Models;
using UniversityManagementSystem.Data;

namespace UniversityManagementSystem.Services;

public class TeacherService
{
    private readonly UniversityDbContext _db;

    public TeacherService(UniversityDbContext db)
    {
        _db = db;
    }

    public Teacher Add(string fullName, string email, int departmentId)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Müəllim adı boş ola bilməz.");

        if (!_db.Departments.Any(d => d.Id == departmentId))
            throw new ArgumentException("Department tapılmadı.");

        Teacher teacher = new Teacher
        {
            FullName = fullName,
            Email = email,
            DepartmentId = departmentId
        };

        _db.Teachers.Add(teacher);
        _db.SaveChanges();

        return teacher;
    }

    public List<Teacher> GetAll()
    {
        return _db.Teachers
            .Include(t => t.Department)
            .AsNoTracking()
            .OrderBy(t => t.Id)
            .ToList();
    }



    public bool Update(int id, string fullName, string email, int departmentId)
    {
        Teacher? teacher = _db.Teachers.Find(id);

        if (teacher == null)
            return false;

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Müəllim adı boş ola bilməz.");

        if (!_db.Departments.Any(d => d.Id == departmentId))
            throw new ArgumentException("Department tapılmadı.");

        teacher.FullName = fullName;
        teacher.Email = email;
        teacher.DepartmentId = departmentId;

        _db.SaveChanges();

        return true;
    }


}
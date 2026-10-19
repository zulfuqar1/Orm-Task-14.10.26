using Microsoft.EntityFrameworkCore;
using Orm_Task_14._10._26.Models;
using UniversityManagementSystem.Data;

namespace UniversityManagementSystem.Services;

public class DepartmentService
{
    private readonly UniversityDbContext _db;

    public DepartmentService(UniversityDbContext db)
    {
        _db = db;
    }

    public Department Add(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Department adı boş ola bilməz.");

        Department department = new Department
        {
            Name = name,
            Description = description
        };

        _db.Departments.Add(department);
        _db.SaveChanges();

        return department;
    }

    public List<Department> GetAll()
    {
        return _db.Departments
            .AsNoTracking()
            .OrderBy(d => d.Id)
            .ToList();
    }

    public List<Department> GetAllRelated()
    {
        return _db.Departments
            .Include(d => d.Teachers)
            .Include(d => d.Courses)
                .ThenInclude(c => c.Teacher)
            .AsNoTracking()
            .ToList();
    }


    public void UpdateDepartment(int id, string name, string? description)
    {
        Department department = _db.Departments.Find(id);

        if (department == null)
            throw new Exception("Department tapılmadı.");

        department.Name = name;
        department.Description = description;

        _db.SaveChanges();
    }




}
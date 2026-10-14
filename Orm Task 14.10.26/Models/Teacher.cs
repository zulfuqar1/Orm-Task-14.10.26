using System;
using System.Collections.Generic;
using System.Text;

namespace Orm_Task_14._10._26.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public List<Course> Courses { get; set; } = new();
    }
}

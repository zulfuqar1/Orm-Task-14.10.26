using System;
using System.Collections.Generic;
using System.Text;

namespace Orm_Task_14._10._26.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Credit { get; set; }
        public int DepartmentId { get; set; }
        public int TeacherId { get; set; }
        public Department Department { get; set; } = null!;
        public Teacher Teacher { get; set; } = null!;
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}

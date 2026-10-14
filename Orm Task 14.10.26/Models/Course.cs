using System;
using System.Collections.Generic;
using System.Text;

namespace Orm_Task_14._10._26.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        //many to many
        public List<Enrollment> Enrollments { get; set; } = new();
    }
}

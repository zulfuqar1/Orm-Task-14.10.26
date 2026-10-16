using System;
using System.Collections.Generic;
using System.Text;

namespace Orm_Task_14._10._26.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}

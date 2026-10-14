using System;
using System.Collections.Generic;
using System.Text;

namespace Orm_Task_14._10._26.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }



        //many to many
        public List<Enrollment> Enrollments { get; set; } = new();

        //public int DepartmentId { get; set; }
        //public Department Department { get; set; }
    }
}
